using neuro_kinetic_backend.Models;

namespace neuro_kinetic_backend.Repositories
{
    public interface IAnalysisResultRepository : IRepository<AnalysisResult>
    {
        Task<AnalysisResult?> GetBySessionIdAsync(string sessionId);
        Task<IEnumerable<AnalysisResult>> GetByAnalysisTypeAsync(AnalysisType type);
        Task<IEnumerable<AnalysisResult>> GetRecentAsync(int count);
    }
}



