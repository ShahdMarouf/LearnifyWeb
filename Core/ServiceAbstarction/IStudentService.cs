using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstarction
{
    public interface IStudentService
    {
        Task<object?> GetStudentSubjectCountAsync(string studentAcademicId);
        Task<StudentDashboardDto?> GetStudentDashboardAsync(string academicId);
        Task<StudentSummaryDto?> GetStudentSummaryAsync(int userId);
    }
}
