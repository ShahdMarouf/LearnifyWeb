using AutoMapper;
using Domain.Models;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ServiceImpementation.mappingProfiles
{
    public class StudentProfile:Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, authResponseDto>();
            CreateMap<StudentCourse,StudentCourseDTO>();
        }
    }
}
