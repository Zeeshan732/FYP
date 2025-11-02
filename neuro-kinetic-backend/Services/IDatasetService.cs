using neuro_kinetic_backend.DTOs.Common;
using neuro_kinetic_backend.DTOs.Dataset;

namespace neuro_kinetic_backend.Services
{
    public interface IDatasetService
    {
        Task<IEnumerable<DatasetDto>> GetAllAsync();
        Task<PagedResult<DatasetDto>> GetPagedAsync(QueryParameters parameters);
        Task<IEnumerable<DatasetDto>> GetPublicAsync();
        Task<PagedResult<DatasetDto>> GetPublicPagedAsync(QueryParameters parameters);
        Task<DatasetDto?> GetByIdAsync(int id);
        Task<DatasetDto> CreateAsync(DatasetDto dto);
        Task<DatasetDto> UpdateAsync(int id, DatasetDto dto);
        Task DeleteAsync(int id);
    }
}

