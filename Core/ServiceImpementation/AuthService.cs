using Domain.Contracts;
using Domain.Models;
using ServiceAbstarction;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceImpementation
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;
        private readonly ITokenSevice _tokenService;

        public AuthService(IUnitOfWork unitOfWork,
                           IPasswordService passwordService,
                           ITokenSevice tokenService)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        public async Task<authResponseDto> LoginAsync(loginDto dto)
        {
            var user = await _unitOfWork.GetRepository<User>()
                .FindAsync(u => u.Email == dto.Email);

            if (user == null)
                throw new Exception("Invalid Email or Password");

            var isPasswordValid = _passwordService.VerifyPassword(
                dto.Password, user.PasswordHash, user.PasswordSalt);

            if (!isPasswordValid)
                throw new Exception("Invalid Email or Password");

            var token = _tokenService.CreateToken(user);

            return new authResponseDto
            {
                Token = token,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}
