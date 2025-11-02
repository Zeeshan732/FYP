using Microsoft.EntityFrameworkCore;
using neuro_kinetic_backend.Data;
using neuro_kinetic_backend.Models;

namespace neuro_kinetic_backend.Repositories
{
    public class PublicationRepository : Repository<Publication>, IPublicationRepository
    {
        public PublicationRepository(ApplicationDbContext context) : base(context)
        {
        }
        
        public async Task<IEnumerable<Publication>> GetFeaturedAsync()
        {
            return await _dbSet.Where(p => p.IsFeatured).OrderByDescending(p => p.CreatedAt).ToListAsync();
        }
        
        public async Task<IEnumerable<Publication>> GetByTypeAsync(PublicationType type)
        {
            return await _dbSet.Where(p => p.Type == type).OrderByDescending(p => p.CreatedAt).ToListAsync();
        }
        
        public async Task<IEnumerable<Publication>> SearchAsync(string searchTerm)
        {
            var term = searchTerm.ToLower();
            return await _dbSet.Where(p => 
                p.Title.ToLower().Contains(term) ||
                (p.Abstract != null && p.Abstract.ToLower().Contains(term)) ||
                (p.Authors != null && p.Authors.ToLower().Contains(term))
            ).OrderByDescending(p => p.CreatedAt).ToListAsync();
        }
    }
}

