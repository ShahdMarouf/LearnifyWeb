using Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IScheduleRepository
    {
        Task<List<ScheduleDto>> GetStudentScheduleAsync(int studentId);
    }
}
