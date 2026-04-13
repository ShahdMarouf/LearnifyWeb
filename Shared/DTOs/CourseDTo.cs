using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class CourseDTo
    {
        public string CourseCode { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public int Credits { get; set; }
    }
}
