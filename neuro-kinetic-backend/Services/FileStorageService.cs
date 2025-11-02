using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace neuro_kinetic_backend.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _basePath;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileStorageService> _logger;
        
        public FileStorageService(IWebHostEnvironment environment, ILogger<FileStorageService> logger, IConfiguration configuration)
        {
            _environment = environment;
            _logger = logger;
            
            // Get base path from configuration or use default
            _basePath = configuration["FileStorage:BasePath"] ?? Path.Combine(_environment.ContentRootPath, "wwwroot", "uploads");
            
            // Ensure directory exists
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }
        
        public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string contentType, string folder)
        {
            try
            {
                // Create folder path
                var folderPath = Path.Combine(_basePath, folder);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                
                // Generate unique file name
                var uniqueFileName = GenerateUniqueFileName(fileName);
                var filePath = Path.Combine(folderPath, uniqueFileName);
                
                // Save file
                using (var fileStreamOutput = new FileStream(filePath, FileMode.Create))
                {
                    await fileStream.CopyToAsync(fileStreamOutput);
                }
                
                // Return relative path for storage in database
                return Path.Combine(folder, uniqueFileName).Replace('\\', '/');
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving file: {FileName}", fileName);
                throw new Exception($"Failed to save file: {ex.Message}");
            }
        }
        
        public async Task<Stream> GetFileAsync(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(_basePath, filePath).Replace('/', Path.DirectorySeparatorChar);
                
                if (!File.Exists(fullPath))
                {
                    throw new FileNotFoundException($"File not found: {filePath}");
                }
                
                return new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving file: {FilePath}", filePath);
                throw;
            }
        }
        
        public async Task<bool> DeleteFileAsync(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(_basePath, filePath).Replace('/', Path.DirectorySeparatorChar);
                
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file: {FilePath}", filePath);
                return false;
            }
        }
        
        public async Task<bool> FileExistsAsync(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(_basePath, filePath).Replace('/', Path.DirectorySeparatorChar);
                return await Task.FromResult(File.Exists(fullPath));
            }
            catch
            {
                return false;
            }
        }
        
        public string GetFileUrl(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return string.Empty;
            
            // Return relative URL for serving files
            return $"/uploads/{filePath.Replace('\\', '/')}";
        }
        
        public async Task<long> GetFileSizeAsync(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(_basePath, filePath).Replace('/', Path.DirectorySeparatorChar);
                
                if (File.Exists(fullPath))
                {
                    var fileInfo = new FileInfo(fullPath);
                    return await Task.FromResult(fileInfo.Length);
                }
                
                return 0;
            }
            catch
            {
                return 0;
            }
        }
        
        private string GenerateUniqueFileName(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var uniqueName = $"{nameWithoutExtension}_{Guid.NewGuid():N}{extension}";
            return uniqueName;
        }
    }
}

