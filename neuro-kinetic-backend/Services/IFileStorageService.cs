namespace neuro_kinetic_backend.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(Stream fileStream, string fileName, string contentType, string folder);
        Task<Stream> GetFileAsync(string filePath);
        Task<bool> DeleteFileAsync(string filePath);
        Task<bool> FileExistsAsync(string filePath);
        string GetFileUrl(string filePath);
        Task<long> GetFileSizeAsync(string filePath);
    }
}



