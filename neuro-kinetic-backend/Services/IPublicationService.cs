using neuro_kinetic_backend.DTOs.Common;
using neuro_kinetic_backend.DTOs.Publication;

namespace neuro_kinetic_backend.Services
{
    public interface IPublicationService
    {
        Task<IEnumerable<PublicationDto>> GetAllAsync();
        Task<PagedResult<PublicationDto>> GetPagedAsync(QueryParameters parameters);
        Task<IEnumerable<PublicationDto>> GetFeaturedAsync();
        Task<PagedResult<PublicationDto>> GetFeaturedPagedAsync(QueryParameters parameters);
        Task<PublicationDto?> GetByIdAsync(int id);
        Task<IEnumerable<PublicationDto>> SearchAsync(string searchTerm);
        Task<PagedResult<PublicationDto>> SearchPagedAsync(string searchTerm, QueryParameters parameters);
        Task<PublicationDto> CreateAsync(PublicationDto dto);
        Task<PublicationDto> UpdateAsync(int id, PublicationDto dto);
        Task DeleteAsync(int id);
    }
}

