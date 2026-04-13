using Domain.Contracts;
using ServiceAbstarction;
using Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceImpementation
{
    public class ScheduleService(IScheduleRepository scheduleRepository) : IScheduleService
    {
        public async Task<List<ScheduleDto>> GetMyScheduleAsync(int userId)
        {
            return await scheduleRepository.GetStudentScheduleAsync(userId);
        }
    }
}
