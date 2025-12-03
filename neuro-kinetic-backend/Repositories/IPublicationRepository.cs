using neuro_kinetic_backend.Models;

namespace neuro_kinetic_backend.Repositories
{
    public interface IPublicationRepository : IRepository<Publication>
    {
        Task<IEnumerable<Publication>> GetFeaturedAsync();
        Task<IEnumerable<Publication>> GetByTypeAsync(PublicationType type);
        Task<IEnumerable<Publication>> SearchAsync(string searchTerm);
    }
}



