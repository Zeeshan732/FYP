namespace neuro_kinetic_backend.DTOs.Metric
{
    public class PerformanceMetricDto
    {
        public int Id { get; set; }
        public string MetricName { get; set; } = string.Empty;
        public string Dataset { get; set; } = string.Empty;
        public decimal Accuracy { get; set; }
        public decimal Precision { get; set; }
        public decimal Recall { get; set; }
        public decimal F1Score { get; set; }
        public decimal? Specificity { get; set; }
        public decimal? Sensitivity { get; set; }
        public decimal? DomainAdaptationDrop { get; set; }
        public string? ValidationMethod { get; set; }
        public string? Notes { get; set; }
        public string? ModelVersion { get; set; }
        public int? FoldNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

