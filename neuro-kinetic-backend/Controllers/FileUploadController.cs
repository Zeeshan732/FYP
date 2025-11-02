using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using neuro_kinetic_backend.DTOs.File;
using neuro_kinetic_backend.Models;
using neuro_kinetic_backend.Repositories;
using neuro_kinetic_backend.Services;
using System.Security.Claims;

namespace neuro_kinetic_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IRepository<UploadedFile> _fileRepository;
        private readonly ILogger<FileUploadController> _logger;
        private readonly IContentTypeProvider _contentTypeProvider;
        
        // Allowed file types and extensions
        private static readonly Dictionary<string, string[]> AllowedFileTypes = new()
        {
            { "voice", new[] { ".wav", ".mp3", ".m4a", ".flac", ".ogg" } },
            { "gait", new[] { ".mp4", ".avi", ".mov", ".mkv", ".webm" } },
            { "video", new[] { ".mp4", ".avi", ".mov", ".mkv", ".webm" } },
            { "image", new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" } }
        };
        
        private const long MaxFileSize = 100 * 1024 * 1024; // 100 MB
        
        public FileUploadController(
            IFileStorageService fileStorageService,
            IRepository<UploadedFile> fileRepository,
            ILogger<FileUploadController> logger,
            IContentTypeProvider contentTypeProvider)
        {
            _fileStorageService = fileStorageService;
            _fileRepository = fileRepository;
            _logger = logger;
            _contentTypeProvider = contentTypeProvider;
        }
        
        [HttpPost("upload")]
        [AllowAnonymous]
        [RequestSizeLimit(MaxFileSize)]
        public async Task<ActionResult<FileUploadResponse>> UploadFile(
            IFormFile file,
            [FromForm] string fileType,
            [FromForm] string? description = null,
            [FromForm] string? sessionId = null)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { message = "No file uploaded" });
                }
                
                // Validate file type
                if (string.IsNullOrEmpty(fileType) || !AllowedFileTypes.ContainsKey(fileType.ToLower()))
                {
                    return BadRequest(new { message = $"Invalid file type. Allowed types: {string.Join(", ", AllowedFileTypes.Keys)}" });
                }
                
                // Validate file extension
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                var allowedExtensions = AllowedFileTypes[fileType.ToLower()];
                
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new { message = $"Invalid file extension. Allowed extensions for {fileType}: {string.Join(", ", allowedExtensions)}" });
                }
                
                // Validate file size
                if (file.Length > MaxFileSize)
                {
                    return BadRequest(new { message = $"File size exceeds maximum allowed size of {MaxFileSize / (1024 * 1024)} MB" });
                }
                
                // Get user ID if authenticated
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                int? userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : null;
                
                // Save file
                var folder = fileType.ToLower();
                var savedPath = await _fileStorageService.SaveFileAsync(
                    file.OpenReadStream(),
                    file.FileName,
                    file.ContentType,
                    folder
                );
                
                // Save file metadata to database
                var uploadedFile = new UploadedFile
                {
                    FileName = file.FileName,
                    FilePath = savedPath,
                    ContentType = file.ContentType,
                    FileSize = file.Length,
                    FileType = fileType.ToLower(),
                    Description = description,
                    SessionId = sessionId,
                    UploadedByUserId = userId,
                    UploadedAt = DateTime.UtcNow,
                    IsActive = true
                };
                
                await _fileRepository.AddAsync(uploadedFile);
                
                // Return response
                var fileUrl = _fileStorageService.GetFileUrl(savedPath);
                var fileSize = await _fileStorageService.GetFileSizeAsync(savedPath);
                
                return Ok(new FileUploadResponse
                {
                    FileName = file.FileName,
                    FilePath = savedPath,
                    FileUrl = fileUrl,
                    ContentType = file.ContentType,
                    FileSize = fileSize,
                    FileType = fileType.ToLower(),
                    UploadedAt = uploadedFile.UploadedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file");
                return StatusCode(500, new { message = "An error occurred while uploading the file" });
            }
        }
        
        [HttpGet("download/{fileId}")]
        [AllowAnonymous]
        public async Task<ActionResult> DownloadFile(int fileId)
        {
            try
            {
                var uploadedFile = await _fileRepository.GetByIdAsync(fileId);
                
                if (uploadedFile == null || !uploadedFile.IsActive)
                {
                    return NotFound(new { message = "File not found" });
                }
                
                var fileStream = await _fileStorageService.GetFileAsync(uploadedFile.FilePath);
                
                if (!_contentTypeProvider.TryGetContentType(uploadedFile.FileName, out var contentType))
                {
                    contentType = "application/octet-stream";
                }
                
                return File(fileStream, contentType, uploadedFile.FileName);
            }
            catch (FileNotFoundException)
            {
                return NotFound(new { message = "File not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file");
                return StatusCode(500, new { message = "An error occurred while downloading the file" });
            }
        }
        
        [HttpGet("url/{fileId}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetFileUrl(int fileId)
        {
            try
            {
                var uploadedFile = await _fileRepository.GetByIdAsync(fileId);
                
                if (uploadedFile == null || !uploadedFile.IsActive)
                {
                    return NotFound(new { message = "File not found" });
                }
                
                var fileUrl = _fileStorageService.GetFileUrl(uploadedFile.FilePath);
                
                return Ok(new { fileUrl = fileUrl, fileName = uploadedFile.FileName, fileSize = uploadedFile.FileSize });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting file URL");
                return StatusCode(500, new { message = "An error occurred while getting file URL" });
            }
        }
        
        [HttpGet("session/{sessionId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<FileUploadResponse>>> GetFilesBySession(string sessionId)
        {
            try
            {
                var files = await _fileRepository.FindAsync(f => f.SessionId == sessionId && f.IsActive);
                var result = files.Select(f => new FileUploadResponse
                {
                    FileName = f.FileName,
                    FilePath = f.FilePath,
                    FileUrl = _fileStorageService.GetFileUrl(f.FilePath),
                    ContentType = f.ContentType,
                    FileSize = f.FileSize,
                    FileType = f.FileType,
                    UploadedAt = f.UploadedAt
                });
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving files by session");
                return StatusCode(500, new { message = "An error occurred while retrieving files" });
            }
        }
        
        [HttpDelete("{fileId}")]
        [Authorize]
        public async Task<ActionResult> DeleteFile(int fileId)
        {
            try
            {
                var uploadedFile = await _fileRepository.GetByIdAsync(fileId);
                
                if (uploadedFile == null)
                {
                    return NotFound(new { message = "File not found" });
                }
                
                // Check if user has permission (owner or admin)
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var isAdmin = User.IsInRole("Admin");
                
                if (!isAdmin && (userIdClaim == null || int.Parse(userIdClaim.Value) != uploadedFile.UploadedByUserId))
                {
                    return Forbid();
                }
                
                // Delete file from storage
                await _fileStorageService.DeleteFileAsync(uploadedFile.FilePath);
                
                // Soft delete (mark as inactive)
                uploadedFile.IsActive = false;
                await _fileRepository.UpdateAsync(uploadedFile);
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file");
                return StatusCode(500, new { message = "An error occurred while deleting the file" });
            }
        }
        
        [HttpGet("types")]
        [AllowAnonymous]
        public ActionResult GetAllowedFileTypes()
        {
            return Ok(new
            {
                allowedTypes = AllowedFileTypes,
                maxFileSize = MaxFileSize,
                maxFileSizeMB = MaxFileSize / (1024 * 1024)
            });
        }
    }
}

