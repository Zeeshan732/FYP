using neuro_kinetic_backend.DTOs.Common;
using neuro_kinetic_backend.DTOs.CrossValidation;

namespace neuro_kinetic_backend.Services
{
    public interface ICrossValidationService
    {
        Task<IEnumerable<CrossValidationResultDto>> GetAllAsync();
        Task<PagedResult<CrossValidationResultDto>> GetPagedAsync(QueryParameters parameters);
        Task<CrossValidationResultDto?> GetByIdAsync(int id);
        Task<IEnumerable<CrossValidationResultDto>> GetByDatasetAsync(string datasetName);
        Task<PagedResult<CrossValidationResultDto>> GetByDatasetPagedAsync(string datasetName, QueryParameters parameters);
        Task<CrossValidationAggregatedDto> GetAggregatedByDatasetAsync(string datasetName);
        Task<CrossValidationResultDto> CreateAsync(CrossValidationResultDto dto);
    }
}

