using Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceAbstarction
{
    public interface IScheduleService
    {
        Task<List<ScheduleDto>> GetMyScheduleAsync(int userId);
    }
}
