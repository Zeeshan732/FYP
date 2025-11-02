using neuro_kinetic_backend.DTOs.Common;
using neuro_kinetic_backend.DTOs.Metric;

namespace neuro_kinetic_backend.Services
{
    public interface IMetricService
    {
        Task<IEnumerable<PerformanceMetricDto>> GetAllAsync();
        Task<PagedResult<PerformanceMetricDto>> GetPagedAsync(QueryParameters parameters);
        Task<PerformanceMetricDto?> GetByIdAsync(int id);
        Task<IEnumerable<PerformanceMetricDto>> GetByDatasetAsync(string dataset);
        Task<PagedResult<PerformanceMetricDto>> GetByDatasetPagedAsync(string dataset, QueryParameters parameters);
        Task<PerformanceMetricDto> CreateAsync(PerformanceMetricDto dto);
        Task<IEnumerable<PerformanceMetricDto>> GetDashboardMetricsAsync();
        Task<MetricsDashboardDto> GetDashboardAggregatedAsync();
    }
}

