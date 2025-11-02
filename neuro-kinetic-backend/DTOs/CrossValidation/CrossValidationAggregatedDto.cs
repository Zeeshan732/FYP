namespace neuro_kinetic_backend.DTOs.CrossValidation
{
    public class CrossValidationAggregatedDto
    {
        public string DatasetName { get; set; } = string.Empty;
        public int TotalFolds { get; set; }
        public decimal AverageAccuracy { get; set; }
        public decimal AveragePrecision { get; set; }
        public decimal AverageRecall { get; set; }
        public decimal AverageF1Score { get; set; }
        public decimal StdDevAccuracy { get; set; }
        public decimal StdDevPrecision { get; set; }
        public decimal StdDevRecall { get; set; }
        public decimal StdDevF1Score { get; set; }
        public List<CrossValidationResultDto> Folds { get; set; } = new();
        public ValidationSummaryDto Summary { get; set; } = new();
    }
    
    public class ValidationSummaryDto
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal Median { get; set; }
        public decimal Q1 { get; set; }
        public decimal Q3 { get; set; }
    }
}

