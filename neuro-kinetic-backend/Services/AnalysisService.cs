using neuro_kinetic_backend.DTOs.Analysis;
using neuro_kinetic_backend.Models;
using neuro_kinetic_backend.Repositories;

namespace neuro_kinetic_backend.Services
{
    public class AnalysisService : IAnalysisService
    {
        private readonly IAnalysisResultRepository _analysisRepository;
        
        public AnalysisService(IAnalysisResultRepository analysisRepository)
        {
            _analysisRepository = analysisRepository;
        }
        
        public async Task<AnalysisResponse> ProcessAnalysisAsync(AnalysisRequest request)
        {
            // Simulate analysis processing (since this is a demo/research platform)
            var analysisType = DetermineAnalysisType(request.HasVoiceData, request.HasGaitData);
            
            // Generate simulated prediction scores
            var random = new Random();
            var predictionScore = (decimal)random.NextDouble();
            var confidenceScore = (decimal)(0.7 + random.NextDouble() * 0.3); // 0.7 to 1.0
            
            var predictedClass = predictionScore > 0.5m 
                ? PredictionClass.ParkinsonPositive 
                : predictionScore < 0.3m 
                    ? PredictionClass.Healthy 
                    : PredictionClass.Uncertain;
            
            var analysisResult = new AnalysisResult
            {
                SessionId = request.SessionId,
                AnalysisType = analysisType,
                HasVoiceData = request.HasVoiceData,
                HasGaitData = request.HasGaitData,
                PredictionScore = predictionScore,
                ConfidenceScore = confidenceScore,
                PredictedClass = predictedClass,
                VoiceFeaturesJson = request.VoiceDataJson,
                GaitFeaturesJson = request.GaitDataJson,
                WaveformDataJson = request.WaveformDataJson,
                SkeletonDataJson = request.SkeletonDataJson,
                CreatedAt = DateTime.UtcNow,
                IsSimulation = true
            };
            
            await _analysisRepository.AddAsync(analysisResult);
            
            return MapToResponse(analysisResult);
        }
        
        public async Task<AnalysisResponse?> GetAnalysisBySessionIdAsync(string sessionId)
        {
            var result = await _analysisRepository.GetBySessionIdAsync(sessionId);
            return result != null ? MapToResponse(result) : null;
        }
        
        public async Task<IEnumerable<AnalysisResponse>> GetRecentAnalysesAsync(int count)
        {
            var results = await _analysisRepository.GetRecentAsync(count);
            return results.Select(MapToResponse);
        }
        
        private AnalysisType DetermineAnalysisType(bool hasVoice, bool hasGait)
        {
            if (hasVoice && hasGait)
                return AnalysisType.MultiModal;
            if (hasVoice)
                return AnalysisType.VoiceOnly;
            if (hasGait)
                return AnalysisType.GaitOnly;
            return AnalysisType.MultiModal; // Default
        }
        
        private AnalysisResponse MapToResponse(AnalysisResult result)
        {
            return new AnalysisResponse
            {
                SessionId = result.SessionId,
                AnalysisType = result.AnalysisType.ToString(),
                PredictionScore = result.PredictionScore,
                ConfidenceScore = result.ConfidenceScore,
                PredictedClass = result.PredictedClass?.ToString(),
                VoiceFeaturesJson = result.VoiceFeaturesJson,
                GaitFeaturesJson = result.GaitFeaturesJson,
                WaveformDataJson = result.WaveformDataJson,
                SkeletonDataJson = result.SkeletonDataJson,
                CreatedAt = result.CreatedAt,
                IsSimulation = result.IsSimulation
            };
        }
    }
}

