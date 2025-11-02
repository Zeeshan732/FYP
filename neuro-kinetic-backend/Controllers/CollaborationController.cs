using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using neuro_kinetic_backend.DTOs.Collaboration;
using neuro_kinetic_backend.Models;
using neuro_kinetic_backend.Repositories;
using neuro_kinetic_backend.Services;

namespace neuro_kinetic_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollaborationController : ControllerBase
    {
        private readonly IRepository<CollaborationRequest> _repository;
        private readonly ILogger<CollaborationController> _logger;
        private readonly IEmailService _emailService;
        
        public CollaborationController(IRepository<CollaborationRequest> repository, ILogger<CollaborationController> logger, IEmailService emailService)
        {
            _repository = repository;
            _logger = logger;
            _emailService = emailService;
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CollaborationRequestDto>>> GetAll()
        {
            try
            {
                var requests = await _repository.GetAllAsync();
                return Ok(requests.OrderByDescending(r => r.CreatedAt).Select(MapToDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving collaboration requests");
                return StatusCode(500, new { message = "An error occurred while retrieving collaboration requests" });
            }
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CollaborationRequestDto>> GetById(int id)
        {
            try
            {
                var request = await _repository.GetByIdAsync(id);
                if (request == null)
                    return NotFound(new { message = "Collaboration request not found" });
                
                return Ok(MapToDto(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving collaboration request");
                return StatusCode(500, new { message = "An error occurred while retrieving collaboration request" });
            }
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<CollaborationRequestDto>> Create([FromBody] CreateCollaborationRequestDto dto)
        {
            try
            {
                var request = new CollaborationRequest
                {
                    InstitutionName = dto.InstitutionName,
                    ContactName = dto.ContactName,
                    ContactEmail = dto.ContactEmail,
                    ContactPhone = dto.ContactPhone,
                    ProposalDescription = dto.ProposalDescription,
                    CollaborationType = dto.CollaborationType,
                    Status = RequestStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };
                
                await _repository.AddAsync(request);
                
                // Send notification email (non-blocking)
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _emailService.SendCollaborationRequestEmailAsync(
                            request.ContactEmail, 
                            request.InstitutionName, 
                            request.ContactName);
                    }
                    catch (Exception ex)
                    {
                        // Log but don't fail request creation if email fails
                        _logger.LogError(ex, "Error sending collaboration request email");
                    }
                });
                
                return CreatedAtAction(nameof(GetById), new { id = request.Id }, MapToDto(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating collaboration request");
                return StatusCode(500, new { message = "An error occurred while creating collaboration request" });
            }
        }
        
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CollaborationRequestDto>> UpdateStatus(int id, [FromBody] UpdateStatusDto dto)
        {
            try
            {
                var request = await _repository.GetByIdAsync(id);
                if (request == null)
                    return NotFound(new { message = "Collaboration request not found" });
                
                request.Status = Enum.Parse<RequestStatus>(dto.Status);
                request.RespondedAt = DateTime.UtcNow;
                request.ResponseNotes = dto.ResponseNotes;
                
                await _repository.UpdateAsync(request);
                
                // Send response email (non-blocking)
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _emailService.SendCollaborationResponseEmailAsync(
                            request.ContactEmail,
                            request.InstitutionName,
                            dto.Status,
                            dto.ResponseNotes);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error sending collaboration response email");
                    }
                });
                
                return Ok(MapToDto(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating collaboration request status");
                return StatusCode(500, new { message = "An error occurred while updating collaboration request" });
            }
        }
        
        private CollaborationRequestDto MapToDto(CollaborationRequest request)
        {
            return new CollaborationRequestDto
            {
                Id = request.Id,
                InstitutionName = request.InstitutionName,
                ContactName = request.ContactName,
                ContactEmail = request.ContactEmail,
                ContactPhone = request.ContactPhone,
                ProposalDescription = request.ProposalDescription,
                CollaborationType = request.CollaborationType,
                Status = request.Status.ToString(),
                CreatedAt = request.CreatedAt
            };
        }
    }
    
    public class UpdateStatusDto
    {
        public string Status { get; set; } = string.Empty;
        public string? ResponseNotes { get; set; }
    }
}

