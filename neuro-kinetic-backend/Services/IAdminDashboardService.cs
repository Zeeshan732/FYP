using neuro_kinetic_backend.DTOs.TestRecord;

namespace neuro_kinetic_backend.Services
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardAnalyticsDto> GetAnalyticsAsync();
    }
}

