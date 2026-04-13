using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class CourseOfferingDTO
    {
        public string CourseCode { get; set; } = null!;
        public string Semester { get; set; } = null!;
        public string AcademicYear { get; set; } = null!;
    }
}
