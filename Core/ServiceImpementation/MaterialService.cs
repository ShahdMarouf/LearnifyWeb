using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ServiceAbstarction;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceImpementation
{
    public class MaterialService : IMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStudentRepository _studentRepository;

        public MaterialService(
            IUnitOfWork unitOfWork,
            IWebHostEnvironment webHostEnvironment,
            IStudentRepository studentRepository)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _studentRepository = studentRepository;
        }

        public async Task<MaterialUploadResult> UploadMaterialAsync(int instructorId, UploadMaterialDto dto)
        {
            // 1. Validate File
            if (dto.File == null || dto.File.Length == 0) return MaterialUploadResult.InvalidFile;

            var extension = Path.GetExtension(dto.File.FileName).ToLower();
            if (extension != ".pdf") return MaterialUploadResult.InvalidFile;

            if (dto.File.Length > 20 * 1024 * 1024) return MaterialUploadResult.InvalidFile; // 20MB

            // 2. Validate Instructor Ownership via CourseOffering table
            var courseOfferingRepo = _unitOfWork.GetRepository<CourseOffering>();
            var courseOffering = await courseOfferingRepo.FindAsync(co =>
                co.CourseCode == dto.CourseCode &&
                co.InstructorId == instructorId);

            if (courseOffering == null) return MaterialUploadResult.NotAuthorized;

            // 3. Save File
            var folderName = Path.Combine("uploads", "materials", dto.CourseCode);
            var pathToSave = Path.Combine(_webHostEnvironment.WebRootPath, folderName);

            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            var fileName = $"{dto.WeekNumber}_{dto.Type}_{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine(folderName, fileName).Replace("\\", "/");

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            // 4. Save to Database
            var material = new CourseMaterial
            {
                Title = dto.Title,
                WeekNumber = dto.WeekNumber,
                Type = dto.Type,
                FilePath = dbPath,
                CourseCode = dto.CourseCode,
                InstructorId = instructorId,
                CreatedAt = DateTime.UtcNow
            };

            var materialRepo = _unitOfWork.GetRepository<CourseMaterial>();
            await materialRepo.AddAsync(material);
            await _unitOfWork.SaveChangesAsync();

            return MaterialUploadResult.Success;
        }

        public async Task<StudentMaterialResponseDto?> GetMaterialsByCourseAsync(int studentId, string courseCode)
        {
            // 1. Check Enrollment via CourseCode
            var student = await _studentRepository.GetStudentWithCoursesAsync(studentId);
            if (student == null || !student.StudentCourses.Any(sc => sc.CourseCode == courseCode))
            {
                return null;
            }

            // 2. Get Materials by CourseCode
            var materialRepo = _unitOfWork.GetRepository<CourseMaterial>();
            var materials = await materialRepo.GetAllAsync();
            var courseMaterials = materials.Where(m => m.CourseCode == courseCode).ToList();

            // 3. Map and Group
            var response = new StudentMaterialResponseDto
            {
                CourseCode = courseCode,
                Weeks = courseMaterials
                    .GroupBy(m => m.WeekNumber)
                    .OrderBy(g => g.Key)
                    .Select(g => new WeekMaterialDto
                    {
                        WeekNumber = g.Key,
                        LectureMaterialId = g.FirstOrDefault(m => m.Type == "Lecture")?.Id,
                        SectionMaterialId = g.FirstOrDefault(m => m.Type == "Section")?.Id
                    }).ToList()
            };

            return response;
        }

        public async Task<(byte[] FileContents, string ContentType, string FileName)?> DownloadMaterialAsync(int studentId, string courseCode, int weekNumber, string type)
        {
            // 1. Check Enrollment
            var student = await _studentRepository.GetStudentWithCoursesAsync(studentId);
            if (student == null || !student.StudentCourses.Any(sc => sc.CourseCode == courseCode))
            {
                return null;
            }

            // 2. Find Material
            var materialRepo = _unitOfWork.GetRepository<CourseMaterial>();
            var material = await materialRepo.FindAsync(m =>
                m.CourseCode == courseCode &&
                m.WeekNumber == weekNumber &&
                m.Type == type);

            if (material == null) return null;

            // 3. Read File
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, material.FilePath);
            if (!System.IO.File.Exists(filePath)) return null;

            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return (bytes, "application/pdf", $"{courseCode}_Week{weekNumber}_{type}.pdf");
        }

        public async Task<InstructorMaterialDashboardDto> GetInstructorDashboardStatsAsync(int instructorId)
        {
            return await _unitOfWork.MaterialRepository.GetInstructorDashboardStatsAsync(instructorId);
        }
    }
}
