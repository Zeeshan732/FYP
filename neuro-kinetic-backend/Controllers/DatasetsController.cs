using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using neuro_kinetic_backend.DTOs.Common;
using neuro_kinetic_backend.DTOs.Dataset;
using neuro_kinetic_backend.Services;

namespace neuro_kinetic_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatasetsController : ControllerBase
    {
        private readonly IDatasetService _datasetService;
        private readonly ILogger<DatasetsController> _logger;
        
        public DatasetsController(IDatasetService datasetService, ILogger<DatasetsController> logger)
        {
            _datasetService = datasetService;
            _logger = logger;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10,
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
                
                var result = await _datasetService.GetPagedAsync(parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving datasets");
                return StatusCode(500, new { message = "An error occurred while retrieving datasets" });
            }
        }
        
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DatasetDto>>> GetAllWithoutPagination()
        {
            try
            {
                var datasets = await _datasetService.GetAllAsync();
                return Ok(datasets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving datasets");
                return StatusCode(500, new { message = "An error occurred while retrieving datasets" });
            }
        }
        
        [HttpGet("public")]
        [AllowAnonymous]
        public async Task<ActionResult> GetPublic([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10,
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
                
                var result = await _datasetService.GetPublicPagedAsync(parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving public datasets");
                return StatusCode(500, new { message = "An error occurred while retrieving public datasets" });
            }
        }
        
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<DatasetDto>> GetById(int id)
        {
            try
            {
                var dataset = await _datasetService.GetByIdAsync(id);
                if (dataset == null)
                    return NotFound(new { message = "Dataset not found" });
                
                return Ok(dataset);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dataset");
                return StatusCode(500, new { message = "An error occurred while retrieving dataset" });
            }
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin,Researcher")]
        public async Task<ActionResult<DatasetDto>> Create([FromBody] DatasetDto dto)
        {
            try
            {
                var dataset = await _datasetService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = dataset.Id }, dataset);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating dataset");
                return StatusCode(500, new { message = "An error occurred while creating dataset" });
            }
        }
        
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Researcher")]
        public async Task<ActionResult<DatasetDto>> Update(int id, [FromBody] DatasetDto dto)
        {
            try
            {
                var dataset = await _datasetService.UpdateAsync(id, dto);
                return Ok(dataset);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating dataset");
                return StatusCode(500, new { message = "An error occurred while updating dataset" });
            }
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _datasetService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting dataset");
                return StatusCode(500, new { message = "An error occurred while deleting dataset" });
            }
        }
    }
}

