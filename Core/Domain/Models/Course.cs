using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Course
    {
        public string CourseCode { get; set; }
        public string FullName { get; set; }
        public int Credits { get; set; }

        public ICollection<CourseOffering> Offerings { get; set; } = new List<CourseOffering>();

    }
}
