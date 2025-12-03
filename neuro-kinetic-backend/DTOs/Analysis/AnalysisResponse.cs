namespace neuro_kinetic_backend.DTOs.Analysis
{
    public class AnalysisResponse
    {
        public string SessionId { get; set; } = string.Empty;
        public string AnalysisType { get; set; } = string.Empty;
        public decimal? PredictionScore { get; set; }
        public decimal? ConfidenceScore { get; set; }
        public string? PredictedClass { get; set; }
        public string? VoiceFeaturesJson { get; set; }
        public string? GaitFeaturesJson { get; set; }
        public string? WaveformDataJson { get; set; }
        public string? SkeletonDataJson { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsSimulation { get; set; }
    }
}



