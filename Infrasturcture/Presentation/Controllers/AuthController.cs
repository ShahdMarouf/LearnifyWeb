using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstarction;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
       
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(loginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var fullName = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(fullName))
                return Unauthorized();

            var firstName = fullName.Split(' ')[0];

            return Ok(new
            {
                message = $"Hello {firstName}",
                fullName = fullName
            });
        }

    }

}

