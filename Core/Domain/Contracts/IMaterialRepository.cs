using Shared.DTOs;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IMaterialRepository
    {
        Task<InstructorMaterialDashboardDto> GetInstructorDashboardStatsAsync(int instructorId);
    }
}
