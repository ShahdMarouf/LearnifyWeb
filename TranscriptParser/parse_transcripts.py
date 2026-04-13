import pdfplumber, re, json, os, typing
from typing import Dict, List, Set, Tuple, Any, Optional, Pattern

PDF_PATH = r"C:\Users\dell\OneDrive\Desktop\transcript_merged.pdf"
OUTPUT_DIR = r"C:\Users\dell\OneDrive\Desktop\Learnify"

# ─── Helpers ────────────────────────────────────────────────────────────────

def parse_semester(sem_str):
    """
    Parse a semester string like 'Fall/2021-2022', 'Spring/2023', 'Summer/2022'
    Returns (season, academic_year)
    """
    sem_str = sem_str.strip()
    m = re.match(r'^(Fall|Spring|Summer)\/(\d{4})(?:-(\d{4}))?', sem_str, re.IGNORECASE)
    if not m:
        return sem_str, ''
    season = m.group(1).capitalize()
    year1  = m.group(2)
    year2  = m.group(3)
    if year2:
        academic_year = f"{year1}-{year2}"
    else:
        y = int(year1)
        academic_year = f"{y-1}-{y}" if season in ('Spring', 'Summer') else f"{year1}-{y+1}"
    return season, academic_year

def clean_course_name(name):
    """
    Remove watermark artefacts from course title.
    Patterns seen:
      - Leading '(E) ' or '(R)* ' etc that bled into name
      - Trailing single capital letters like ' C', ' F', ' I', etc.
      - Trailing 'N ' or similar (watermarked chars that got appended)
    """
    # Remove markers like (E), (R)*, (R) from the beginning
    name = re.sub(r'^\([RE]\)\*?\s*', '', name).strip()
    # Remove trailing single capital letter (watermark) optionally preceded by space
    name = re.sub(r'\s+[A-Z]$', '', name).strip()
    # Also catch trailing 'N ' or similar at end
    name = re.sub(r'^[A-Z]\s+', '', name).strip()
    return name

def extract_course_from_line(line):
    """
    Parse a course line.  The challenge: watermark letters ('U','N','O','F','F','I','C','I','A','L')
    are sometimes merged into course codes or titles by pdfplumber.

    Pattern:
      CODE[markers]  TITLE  3.00  [%]  GRADE  earn  hrs  pts
    """
    # Try to match: code  title  3.00  [%]  grade  ...
    m = re.match(
        r'^([A-Z][A-Z0-9\-\.]+(?:\([RE]\))?(?:\(R\)\*)?)\s+'  # course code
        r'(.*?)\s+'                                              # title (lazy)
        r'(\d+\.\d{2})\s+'                                      # CR (3.00)
        r'(?:(\d+)\s+)?'                                        # % (optional)
        r'([A-Z][A-Z\+\-\*]*|PE|W|P|F|I|D)\s*'               # Grade
        r'(\d+\.\d+)\s+'                                        # GPHRSEarn
        r'(\d+\.\d+)\s+'                                        # Hr.
        r'(\d+\.\d+)',                                          # Points
        line
    )
    if m:
        code  = m.group(1).strip()
        name  = m.group(2).strip()
        grade = m.group(5).strip()
        cr    = float(m.group(3))

        # Clean code: strip (E) and (R)* retake / exclude markers
        base_code = re.sub(r'\(R\)\*|\([RE]\)\*?', '', code).strip()

        # Clean name
        name = clean_course_name(name)

        return {
            'code':      base_code,
            'raw_code':  code,
            'name':      name,
            'cr':        cr,
            'grade':     grade,
        }
    return None

# ─── Regex patterns ──────────────────────────────────────────────────────────
SEMESTER_RE: Pattern = re.compile(r'^(Fall|Spring|Summer)\/\d{4}', re.IGNORECASE)
CUMGPA_RE: Pattern   = re.compile(r'Cumulative GPA:\s*([\d\.]+)', re.IGNORECASE)
ID_NAME_RE: Pattern  = re.compile(r'ID\s*:\s*(\d+)\s+Name\s*:\s*(.+)', re.IGNORECASE)
NOISE_RE: Pattern    = re.compile(
    r'^(UNOfficial Student Transcript|College\s*:|Semester Course Number'
    r'|Page \d+ of \d+|-+$|Current GPA:|$)', re.IGNORECASE
)
WATERMARK_CHAR_RE: Pattern = re.compile(r'^[A-Z]$')  # isolated single uppercase = watermark

# ─── Parse ───────────────────────────────────────────────────────────────────

students = {}        # id -> {id, name, gpa}
courses_dict = {}    # base_code -> {code, name, credits}
offerings = set()    # (code, semester, year)
student_courses = [] # list of dicts
last_cum_gpa = {}    # student_id -> float
current_id = None
current_semester = None
current_year = None

with pdfplumber.open(PDF_PATH) as pdf:
    try:
        pages = pdf.pages
    except (AttributeError, TypeError):
        pages = []

    for page in pages:
        try:
            # Direct property access to bypass broken getattr stubs
            raw_text = page.extract_text() or ""
        except (AttributeError, TypeError):
            raw_text = ""
        text = str(raw_text)
        for raw_line in text.split("\n"):
            line = raw_line.strip()
            if not line:
                continue

            # ── Student header ─────────────────────────────────────────────
            m_id = re.search(ID_NAME_RE, line)
            if m_id:
                current_id   = m_id.group(1).strip()
                current_name = m_id.group(2).strip()
                if current_id not in students:
                    students[current_id] = {
                        'id':   current_id,
                        'name': current_name,
                        'gpa':  0.0,
                    }
                continue

            # ── Skip noise ─────────────────────────────────────────────────
            if re.match(NOISE_RE, line):
                continue
            if re.match(WATERMARK_CHAR_RE, line):
                continue

            # ── Cumulative GPA ─────────────────────────────────────────────
            m_gpa = re.search(CUMGPA_RE, line)
            if m_gpa and current_id:
                last_cum_gpa[current_id] = float(m_gpa.group(1))
                continue

            # ── Semester header ────────────────────────────────────────────
            m_sem = re.match(SEMESTER_RE, line)
            if m_sem:
                sem_token = line.split()[0]
                current_semester, current_year = parse_semester(sem_token)
                continue

            # ── Course line ────────────────────────────────────────────────
            if current_id and current_semester:
                c = extract_course_from_line(line)
                if c:
                    code  = c['code']
                    name  = c['name']
                    grade = c['grade']
                    cr    = c['cr']

                    # Best name: pick the first non-empty one seen for a given code
                    if code not in courses_dict:
                        courses_dict[code] = {
                            'code':    code,
                            'name':    name,
                            'credits': int(cr),
                        }

                    if current_semester and current_year:
                        offerings.add((code, current_semester, current_year))

                    student_courses.append({
                        'StudentAcademicId': current_id,
                        'CourseCode':        code,
                        'Semester':          current_semester,
                        'AcademicYear':      current_year,
                        'Grade':             grade,
                    })

# ── Assign final GPAs ─────────────────────────────────────────────────────
for sid, gpa in last_cum_gpa.items():
    if sid in students:
        students[sid]['gpa'] = gpa

# ─── Build JSON payloads ──────────────────────────────────────────────────

students_json = []
for sid, s in students.items():
    raw_gpa_val = float(s["gpa"])
    # Using string formatting for rounding to satisfy restricted round() stubs
    gpa = float("{:.2f}".format(raw_gpa_val))
    pct = int(gpa * 25)
    students_json.append({
        "FullName":     s['name'],
        "Email":        f"{sid}@must.edu.eg",
        "PasswordHash": "HASH_HERE",
        "PasswordSalt": "SALT_HERE",
        "Role":         "Student",
        "Instructor":   None,
        "GPA":          gpa,
        "StudyHours":   108,
        "Percentage":   pct,
    })

courses_json = sorted(
    [{"CourseCode": v['code'], "CourseName": v['name'], "Credits": v['credits']}
     for v in courses_dict.values()],
    key=lambda x: x['CourseCode']
)

offerings_json = sorted(
    [{"CourseCode": o[0], "Semester": o[1], "AcademicYear": o[2]} for o in offerings],
    key=lambda x: (x['AcademicYear'], x['Semester'], x['CourseCode'])
)

# ─── Write JSON files ──────────────────────────────────────────────────────

files = {
    "Students.json":        students_json,
    "Courses.json":         courses_json,
    "CourseOfferings.json": offerings_json,
    "StudentCourses.json":  student_courses,
}

for fname, data in files.items():
    path = os.path.join(OUTPUT_DIR, fname)
    with open(path, "w", encoding="utf-8") as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
    print(f"Written {len(data):>6} records -> {fname}")

# ─── Summary ──────────────────────────────────────────────────────────────
print(f"\n{'='*50}")
print(f"Students:         {len(students_json)}")
print(f"Unique courses:   {len(courses_json)}")
print(f"Course offerings: {len(offerings_json)}")
print(f"Student courses:  {len(student_courses)}")
print(f"\nFirst 5 students:")
for i in range(min(5, len(students_json))):
    s = students_json[i]
    gpa_val = float(s["GPA"])
    print(f"  {s['Email']:35s} GPA={gpa_val:.2f}  Pct={s['Percentage']}  {s['FullName']}")
print(f"\nAll course codes:")
for c in courses_json:
    print(f"  {c['CourseCode']:20s} {c['CourseName']}")
