using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using neuro_kinetic_backend.DTOs.Common;
using neuro_kinetic_backend.DTOs.CrossValidation;
using neuro_kinetic_backend.Services;

namespace neuro_kinetic_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrossValidationController : ControllerBase
    {
        private readonly ICrossValidationService _service;
        private readonly ILogger<CrossValidationController> _logger;
        
        public CrossValidationController(ICrossValidationService service, ILogger<CrossValidationController> logger)
        {
            _service = service;
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
                
                var result = await _service.GetPagedAsync(parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cross-validation results");
                return StatusCode(500, new { message = "An error occurred while retrieving cross-validation results" });
            }
        }
        
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CrossValidationResultDto>>> GetAllWithoutPagination()
        {
            try
            {
                var results = await _service.GetAllAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cross-validation results");
                return StatusCode(500, new { message = "An error occurred while retrieving cross-validation results" });
            }
        }
        
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CrossValidationResultDto>> GetById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                    return NotFound(new { message = "Cross-validation result not found" });
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cross-validation result");
                return StatusCode(500, new { message = "An error occurred while retrieving cross-validation result" });
            }
        }
        
        [HttpGet("dataset/{datasetName}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetByDataset(string datasetName, [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10, [FromQuery] string? sortBy = null, [FromQuery] string? sortOrder = "asc")
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
                
                var result = await _service.GetByDatasetPagedAsync(datasetName, parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cross-validation results by dataset");
                return StatusCode(500, new { message = "An error occurred while retrieving cross-validation results" });
            }
        }
        
        [HttpGet("dataset/{datasetName}/aggregated")]
        [AllowAnonymous]
        public async Task<ActionResult<CrossValidationAggregatedDto>> GetAggregatedByDataset(string datasetName)
        {
            try
            {
                var result = await _service.GetAggregatedByDatasetAsync(datasetName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving aggregated cross-validation results");
                return StatusCode(500, new { message = "An error occurred while retrieving aggregated cross-validation results" });
            }
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin,Researcher")]
        public async Task<ActionResult<CrossValidationResultDto>> Create([FromBody] CrossValidationResultDto dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating cross-validation result");
                return StatusCode(500, new { message = "An error occurred while creating cross-validation result" });
            }
        }
    }
}

