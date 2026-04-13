using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Instructor:User
    {
              public ICollection<Student> Students { get; set; }
              public string? ProfileImagePath { get; set; }
    }
}

