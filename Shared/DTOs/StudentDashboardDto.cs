using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class StudentDashboardDto
    {
        public int TotalSubjects { get; set; }
        public int StudyHours { get; set; }
        public decimal Gpa { get; set; }
     
    }
}
