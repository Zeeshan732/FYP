namespace neuro_kinetic_backend.DTOs.CrossValidation
{
    public class CrossValidationResultDto
    {
        public int Id { get; set; }
        public string DatasetName { get; set; } = string.Empty;
        public string? ValidationMethod { get; set; }
        public int FoldNumber { get; set; }
        public decimal Accuracy { get; set; }
        public decimal Precision { get; set; }
        public decimal Recall { get; set; }
        public decimal F1Score { get; set; }
        public decimal? DomainAdaptationDrop { get; set; }
        public string? SourceSite { get; set; }
        public string? TargetSite { get; set; }
        public string? ModelVersion { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}



