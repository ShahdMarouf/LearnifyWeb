using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string PasswordSalt { get; set; } = null!;

        public string Role { get; set; } = null!;
        public string? ProfileImagePath { get; set; }
    }
}
