using System.ComponentModel.DataAnnotations;

namespace neuro_kinetic_backend.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(200)]
        public string PasswordHash { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        public UserRole Role { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastLoginAt { get; set; }
        
        public string? Institution { get; set; }
        
        public string? ResearchFocus { get; set; }
    }
    
    public enum UserRole
    {
        Public = 0,
        Researcher = 1,
        MedicalProfessional = 2,
        Admin = 3
    }
}



