namespace neuro_kinetic_backend.DTOs.Metric
{
    public class MetricsDashboardDto
    {
        public int TotalMetrics { get; set; }
        public decimal AverageAccuracy { get; set; }
        public decimal AveragePrecision { get; set; }
        public decimal AverageRecall { get; set; }
        public decimal AverageF1Score { get; set; }
        public decimal BestAccuracy { get; set; }
        public decimal WorstAccuracy { get; set; }
        public List<DatasetMetricsSummaryDto> MetricsByDataset { get; set; } = new();
        public List<PerformanceMetricDto> RecentMetrics { get; set; } = new();
        public MetricsTrendDto Trends { get; set; } = new();
    }
    
    public class DatasetMetricsSummaryDto
    {
        public string Dataset { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal AverageAccuracy { get; set; }
        public decimal AveragePrecision { get; set; }
        public decimal AverageRecall { get; set; }
        public decimal AverageF1Score { get; set; }
        public decimal BestAccuracy { get; set; }
    }
    
    public class MetricsTrendDto
    {
        public List<TimeSeriesDataPoint> AccuracyOverTime { get; set; } = new();
        public decimal Improvement { get; set; }
        public string TrendDirection { get; set; } = string.Empty; // "improving", "stable", "declining"
    }
    
    public class TimeSeriesDataPoint
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string Label { get; set; } = string.Empty;
    }
}



