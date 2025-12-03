namespace neuro_kinetic_backend.DTOs.TestRecord
{
    public class AdminDashboardAnalyticsDto
    {
        public int TotalUsers { get; set; }
        public int TotalTests { get; set; }
        public int PositiveCases { get; set; }
        public int NegativeCases { get; set; }
        public int UncertainCases { get; set; }
        public double AverageAccuracy { get; set; }
        public List<TimeSeriesDataPoint> UsageByDay { get; set; } = new();
        public List<TimeSeriesDataPoint> UsageByMonth { get; set; } = new();
        public List<TimeSeriesDataPoint> UsageByYear { get; set; } = new();
        public List<UserTestRecordDto> RecentTests { get; set; } = new();
        public TestResultsDistributionDto TestResultsDistribution { get; set; } = new();
    }
    
    public class TimeSeriesDataPoint
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public string Label { get; set; } = string.Empty;
    }
    
    public class TestResultsDistributionDto
    {
        public int Positive { get; set; }
        public int Negative { get; set; }
        public int Uncertain { get; set; }
    }
}



