using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class StudentSummaryDto
    {
        public int TotalCredits { get; set; }
        public decimal Gpa { get; set; }
        public double Percentage { get; set; }
    }
}
