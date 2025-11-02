namespace neuro_kinetic_backend.DTOs.Content
{
    public class TechnologyContentDto
    {
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public List<ContentSectionDto> Sections { get; set; } = new();
        public TechnologyMetricsDto ValidationResults { get; set; } = new();
        public List<ArchitectureDiagramDto> Diagrams { get; set; } = new();
    }
    
    public class ContentSectionDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int Order { get; set; }
        public List<string>? BulletPoints { get; set; }
    }
    
    public class TechnologyMetricsDto
    {
        public decimal OverallAccuracy { get; set; }
        public decimal AveragePrecision { get; set; }
        public decimal AverageRecall { get; set; }
        public decimal AverageF1Score { get; set; }
        public CrossValidationSummaryDto? CrossValidation { get; set; }
        public List<DatasetPerformanceDto> DatasetPerformance { get; set; } = new();
    }
    
    public class CrossValidationSummaryDto
    {
        public int TotalFolds { get; set; }
        public decimal AverageAccuracy { get; set; }
        public decimal StdDevAccuracy { get; set; }
    }
    
    public class DatasetPerformanceDto
    {
        public string DatasetName { get; set; } = string.Empty;
        public decimal Accuracy { get; set; }
        public int SampleCount { get; set; }
    }
    
    public class ArchitectureDiagramDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public List<ComponentDto> Components { get; set; } = new();
    }
    
    public class ComponentDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "Input", "Processing", "Output"
    }
}

