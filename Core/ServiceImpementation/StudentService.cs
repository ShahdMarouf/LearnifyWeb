using Domain.Contracts;
using ServiceAbstarction;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceImpementation
{
    public class StudentService(IStudentRepository studentRepository) : IStudentService
    {
        // Service implementation for Student operations
        public async Task<object?> GetStudentSubjectCountAsync(string studentAcademicId)
        {
            if (!int.TryParse(studentAcademicId, out int academicId))
            {
                // Optionally throw an exception or return null based on error handling strategy. 
                // For this task, returning null will likely result in 404 which is acceptable, 
                // but usually Bad Request is better. However, per requirements, we'll focus on the core logic.
                // Will treat invalid format as "Student Not Found" or handle in controller.
                // Let's return null to signify "Not Processable/Found".
                return null;
            }

            var student = await studentRepository.GetByAcademicIdAsync(academicId);
            if (student == null)
            {
                return null;
            }

            return new
            {
                StudentAcademicId = studentAcademicId,
                TotalSubjects = student.StudentCourses.Count
            };
        }


        public async Task<StudentDashboardDto?> GetStudentDashboardAsync(string academicId)
        {
            if (!int.TryParse(academicId, out int id)) return null;

            var student = await studentRepository.GetByAcademicIdAsync(id);
            if (student == null) return null;

            return new StudentDashboardDto
            {
                TotalSubjects = student.StudentCourses.Count,
                StudyHours = student.StudyHours,
                Gpa = student.GPA,

            };
        }

        public async Task<StudentSummaryDto?> GetStudentSummaryAsync(int userId)
        {
            var student = await studentRepository.GetStudentWithCoursesAsync(userId);
            if (student == null) return null;

            int totalCredits = student.StudentCourses.Count * 3; // Simplified for now since navigation is gone

            return new StudentSummaryDto
            {
                TotalCredits = totalCredits,
                Gpa = student.GPA,
                Percentage = (double)student.Percentage
            };
        }
    }
}
