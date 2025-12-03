namespace neuro_kinetic_backend.DTOs.File
{
    public class FileUploadRequest
    {
        public string FileType { get; set; } = string.Empty; // "voice", "gait", "image", "video"
        public string? Description { get; set; }
        public string? SessionId { get; set; }
    }
}



