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
    public class StudentCourseService(IStudentCourseRepository repo) : IStudentCourseService
    {
        public async Task<IEnumerable<StudentCourseDTO>> GetMyCourses(
     int userId,
     string semester,
     string academicYear)
        {
            var studentCourses =
                await repo.GetMyCoursesAsync(userId, semester, academicYear);

            return studentCourses.Select(sc => new StudentCourseDTO
            {
                CourseCode = sc.CourseCode,
                Grade = sc.Grade,
                Semester = sc.Semester,
                AcademicYear = sc.AcademicYear
            });
        }



    }
}
