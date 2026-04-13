using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class StudentSeedDto
    {
            public string FullName { get; set; }
            public string Email { get; set; }
            public decimal GPA {  get; set; }
            public int StudyHours {  get; set; }
            public decimal Percentage { get; set; }

    }
}
