using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstarction
{
    public interface IAuthService
    {
        Task <authResponseDto> LoginAsync(loginDto dto);

    }
}
