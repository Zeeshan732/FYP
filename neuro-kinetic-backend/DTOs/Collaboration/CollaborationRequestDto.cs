namespace neuro_kinetic_backend.DTOs.Collaboration
{
    public class CollaborationRequestDto
    {
        public int Id { get; set; }
        public string InstitutionName { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string? ContactPhone { get; set; }
        public string? ProposalDescription { get; set; }
        public string? CollaborationType { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
    
    public class CreateCollaborationRequestDto
    {
        public string InstitutionName { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string? ContactPhone { get; set; }
        public string? ProposalDescription { get; set; }
        public string? CollaborationType { get; set; }
    }
}



