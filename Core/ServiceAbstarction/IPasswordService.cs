using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstarction
{
    public interface IPasswordService
    {
        void CreatePasswordHash(string password, out string hash, out string salt);
        bool VerifyPassword(string password, string storedHash, string storedSalt);
    }
}

