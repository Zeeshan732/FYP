using neuro_kinetic_backend.DTOs.Analysis;

namespace neuro_kinetic_backend.Services
{
    public interface IAnalysisService
    {
        Task<AnalysisResponse> ProcessAnalysisAsync(AnalysisRequest request);
        Task<AnalysisResponse?> GetAnalysisBySessionIdAsync(string sessionId);
        Task<IEnumerable<AnalysisResponse>> GetRecentAnalysesAsync(int count);
    }
}



