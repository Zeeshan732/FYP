using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace neuro_kinetic_backend.Models
{
    public class PerformanceMetric
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string MetricName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(200)]
        public string Dataset { get; set; } = string.Empty;
        
        public decimal Accuracy { get; set; }
        
        public decimal Precision { get; set; }
        
        public decimal Recall { get; set; }
        
        public decimal F1Score { get; set; }
        
        public decimal? Specificity { get; set; }
        
        public decimal? Sensitivity { get; set; }
        
        public decimal? DomainAdaptationDrop { get; set; }
        
        [MaxLength(50)]
        public string? ValidationMethod { get; set; }
        
        [MaxLength(500)]
        public string? Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [MaxLength(100)]
        public string? ModelVersion { get; set; }
        
        public int? FoldNumber { get; set; }
    }
}



