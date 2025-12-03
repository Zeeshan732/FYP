using System.ComponentModel.DataAnnotations;

namespace neuro_kinetic_backend.Models
{
    public class CrossValidationResult
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string DatasetName { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string? ValidationMethod { get; set; }
        
        public int FoldNumber { get; set; }
        
        public decimal Accuracy { get; set; }
        
        public decimal Precision { get; set; }
        
        public decimal Recall { get; set; }
        
        public decimal F1Score { get; set; }
        
        public decimal? DomainAdaptationDrop { get; set; }
        
        [MaxLength(100)]
        public string? SourceSite { get; set; }
        
        [MaxLength(100)]
        public string? TargetSite { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [MaxLength(100)]
        public string? ModelVersion { get; set; }
        
        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}



