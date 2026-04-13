using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ServiceAbstarction;
using Shared.DTOs;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceImpementation
{
    public class InstructorService : IInstructorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;
        private readonly ITokenSevice _tokenService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public InstructorService(IUnitOfWork unitOfWork,
                                 IPasswordService passwordService,
                                 ITokenSevice tokenService,
                                 IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<InstructorLoginResponseDto?> LoginAsync(InstructorLoginDto dto)
        {
            var user = await _unitOfWork.GetRepository<User>()
                .FindAsync(u => u.Email == dto.Email);

            if (user == null)
            {
                throw new Exception("Invalid Email or Password");
            }

            if (user.Role != "Instructor")
            {
                throw new Exception("Forbidden: User is not an Instructor");
            }

            var isPasswordValid = _passwordService.VerifyPassword(
                dto.Password, user.PasswordHash, user.PasswordSalt);

            if (!isPasswordValid)
            {
                throw new Exception("Invalid Email or Password");
            }

            var token = _tokenService.CreateToken(user);

            return new InstructorLoginResponseDto
            {
                Token = token,
                FullName = user.FullName,
                Role = user.Role
            };
        }

        public async Task UpdateProfileAsync(int instructorId, UpdateInstructorDto dto)
        {
            var instructor = await _unitOfWork.GetRepository<User>()
                .FindAsync(u => u.Id == instructorId);

            if (instructor == null)
            {
                throw new Exception("Instructor not found");
            }

            if (instructor.Email != dto.Email)
            {
                var existingUser = await _unitOfWork.GetRepository<User>()
                    .FindAsync(u => u.Email == dto.Email);

                if (existingUser != null)
                {
                    throw new Exception("Email is already taken by another user");
                }
            }

            instructor.FullName = dto.FullName;
            instructor.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.NewPassword))
            {
                _passwordService.CreatePasswordHash(dto.NewPassword, out string hash, out string salt);
                instructor.PasswordHash = hash;
                instructor.PasswordSalt = salt;
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<string> UploadPhotoAsync(int instructorId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("No file uploaded");

            if (file.Length > 2 * 1024 * 1024)
                throw new Exception("File size exceeds 2MB limit");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                throw new Exception("Only .jpg, .jpeg, and .png files are allowed");

            var instructor = await _unitOfWork.GetRepository<User>().FindAsync(u => u.Id == instructorId);
            if (instructor == null)
                throw new Exception("Instructor not found");

            // Delete old photo if exists
            if (!string.IsNullOrEmpty(instructor.ProfileImagePath))
            {
                var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, instructor.ProfileImagePath.TrimStart('/'));
                if (File.Exists(oldPath))
                    File.Delete(oldPath);
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "instructors");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            instructor.ProfileImagePath = $"/uploads/instructors/{fileName}";
            await _unitOfWork.SaveChangesAsync();

            return instructor.ProfileImagePath;
        }

        public async Task DeletePhotoAsync(int instructorId)
        {
            var instructor = await _unitOfWork.GetRepository<User>().FindAsync(u => u.Id == instructorId);
            if (instructor == null)
                throw new Exception("Instructor not found");

            if (!string.IsNullOrEmpty(instructor.ProfileImagePath))
            {
                var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, instructor.ProfileImagePath.TrimStart('/'));
                if (File.Exists(fullPath))
                    File.Delete(fullPath);

                instructor.ProfileImagePath = null;
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<string?> GetPhotoUrlAsync(int instructorId)
        {
            var instructor = await _unitOfWork.GetRepository<User>().FindAsync(u => u.Id == instructorId);
            return instructor?.ProfileImagePath;
        }

        public async Task<IEnumerable<InstructorCourseStudentDto>> GetStudentsByCourseAsync(string courseCode)
        {
            return await _unitOfWork.StudentCourseRepository.GetStudentsByCourseAsync(courseCode);
        }
    }
}
