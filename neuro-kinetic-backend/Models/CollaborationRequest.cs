using System.ComponentModel.DataAnnotations;

namespace neuro_kinetic_backend.Models
{
    public class CollaborationRequest
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string InstitutionName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string ContactName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string ContactEmail { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string? ContactPhone { get; set; }
        
        [MaxLength(1000)]
        public string? ProposalDescription { get; set; }
        
        [MaxLength(200)]
        public string? CollaborationType { get; set; }
        
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? RespondedAt { get; set; }
        
        [MaxLength(1000)]
        public string? ResponseNotes { get; set; }
        
        public int? RespondedByUserId { get; set; }
    }
    
    public enum RequestStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        UnderReview = 3
    }
}



