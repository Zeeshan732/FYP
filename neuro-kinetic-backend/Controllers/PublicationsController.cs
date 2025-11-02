using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using neuro_kinetic_backend.DTOs.Common;
using neuro_kinetic_backend.DTOs.Publication;
using neuro_kinetic_backend.Services;

namespace neuro_kinetic_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicationsController : ControllerBase
    {
        private readonly IPublicationService _publicationService;
        private readonly ILogger<PublicationsController> _logger;
        
        public PublicationsController(IPublicationService publicationService, ILogger<PublicationsController> logger)
        {
            _publicationService = publicationService;
            _logger = logger;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, 
            [FromQuery] string? sortBy = null, [FromQuery] string? sortOrder = "asc", [FromQuery] string? searchTerm = null)
        {
            try
            {
                var parameters = new QueryParameters
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    SearchTerm = searchTerm
                };
                
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    var result = await _publicationService.SearchPagedAsync(searchTerm, parameters);
                    return Ok(result);
                }
                
                var publications = await _publicationService.GetPagedAsync(parameters);
                return Ok(publications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving publications");
                return StatusCode(500, new { message = "An error occurred while retrieving publications" });
            }
        }
        
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PublicationDto>>> GetAllWithoutPagination()
        {
            try
            {
                var publications = await _publicationService.GetAllAsync();
                return Ok(publications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving publications");
                return StatusCode(500, new { message = "An error occurred while retrieving publications" });
            }
        }
        
        [HttpGet("featured")]
        [AllowAnonymous]
        public async Task<ActionResult> GetFeatured([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null, [FromQuery] string? sortOrder = "asc")
        {
            try
            {
                var parameters = new QueryParameters
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SortBy = sortBy,
                    SortOrder = sortOrder
                };
                
                var result = await _publicationService.GetFeaturedPagedAsync(parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving featured publications");
                return StatusCode(500, new { message = "An error occurred while retrieving featured publications" });
            }
        }
        
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<PublicationDto>> GetById(int id)
        {
            try
            {
                var publication = await _publicationService.GetByIdAsync(id);
                if (publication == null)
                    return NotFound(new { message = "Publication not found" });
                
                return Ok(publication);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving publication");
                return StatusCode(500, new { message = "An error occurred while retrieving publication" });
            }
        }
        
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PublicationDto>>> Search([FromQuery] string query)
        {
            try
            {
                var publications = await _publicationService.SearchAsync(query);
                return Ok(publications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching publications");
                return StatusCode(500, new { message = "An error occurred while searching publications" });
            }
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin,Researcher")]
        public async Task<ActionResult<PublicationDto>> Create([FromBody] PublicationDto dto)
        {
            try
            {
                var publication = await _publicationService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = publication.Id }, publication);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating publication");
                return StatusCode(500, new { message = "An error occurred while creating publication" });
            }
        }
        
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Researcher")]
        public async Task<ActionResult<PublicationDto>> Update(int id, [FromBody] PublicationDto dto)
        {
            try
            {
                var publication = await _publicationService.UpdateAsync(id, dto);
                return Ok(publication);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating publication");
                return StatusCode(500, new { message = "An error occurred while updating publication" });
            }
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _publicationService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting publication");
                return StatusCode(500, new { message = "An error occurred while deleting publication" });
            }
        }
    }
}

