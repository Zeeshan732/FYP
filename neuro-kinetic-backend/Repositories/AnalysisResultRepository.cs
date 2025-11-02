using Microsoft.EntityFrameworkCore;
using neuro_kinetic_backend.Data;
using neuro_kinetic_backend.Models;

namespace neuro_kinetic_backend.Repositories
{
    public class AnalysisResultRepository : Repository<AnalysisResult>, IAnalysisResultRepository
    {
        public AnalysisResultRepository(ApplicationDbContext context) : base(context)
        {
        }
        
        public async Task<AnalysisResult?> GetBySessionIdAsync(string sessionId)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.SessionId == sessionId);
        }
        
        public async Task<IEnumerable<AnalysisResult>> GetByAnalysisTypeAsync(AnalysisType type)
        {
            return await _dbSet.Where(a => a.AnalysisType == type)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<AnalysisResult>> GetRecentAsync(int count)
        {
            return await _dbSet.OrderByDescending(a => a.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }
}

