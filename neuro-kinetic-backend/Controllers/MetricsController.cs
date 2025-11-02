using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using neuro_kinetic_backend.DTOs.Common;
using neuro_kinetic_backend.DTOs.Metric;
using neuro_kinetic_backend.Services;

namespace neuro_kinetic_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetricsController : ControllerBase
    {
        private readonly IMetricService _metricService;
        private readonly ILogger<MetricsController> _logger;
        
        public MetricsController(IMetricService metricService, ILogger<MetricsController> logger)
        {
            _metricService = metricService;
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
                
                var result = await _metricService.GetPagedAsync(parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving metrics");
                return StatusCode(500, new { message = "An error occurred while retrieving metrics" });
            }
        }
        
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PerformanceMetricDto>>> GetAllWithoutPagination()
        {
            try
            {
                var metrics = await _metricService.GetAllAsync();
                return Ok(metrics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving metrics");
                return StatusCode(500, new { message = "An error occurred while retrieving metrics" });
            }
        }
        
        [HttpGet("dashboard")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PerformanceMetricDto>>> GetDashboard()
        {
            try
            {
                var metrics = await _metricService.GetDashboardMetricsAsync();
                return Ok(metrics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard metrics");
                return StatusCode(500, new { message = "An error occurred while retrieving dashboard metrics" });
            }
        }
        
        [HttpGet("dashboard/aggregated")]
        [AllowAnonymous]
        public async Task<ActionResult> GetDashboardAggregated()
        {
            try
            {
                var dashboard = await _metricService.GetDashboardAggregatedAsync();
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving aggregated dashboard metrics");
                return StatusCode(500, new { message = "An error occurred while retrieving aggregated dashboard metrics" });
            }
        }
        
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<PerformanceMetricDto>> GetById(int id)
        {
            try
            {
                var metric = await _metricService.GetByIdAsync(id);
                if (metric == null)
                    return NotFound(new { message = "Metric not found" });
                
                return Ok(metric);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving metric");
                return StatusCode(500, new { message = "An error occurred while retrieving metric" });
            }
        }
        
        [HttpGet("dataset/{dataset}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetByDataset(string dataset, [FromQuery] int pageNumber = 1, 
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
                
                var result = await _metricService.GetByDatasetPagedAsync(dataset, parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving metrics by dataset");
                return StatusCode(500, new { message = "An error occurred while retrieving metrics" });
            }
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin,Researcher")]
        public async Task<ActionResult<PerformanceMetricDto>> Create([FromBody] PerformanceMetricDto dto)
        {
            try
            {
                var metric = await _metricService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = metric.Id }, metric);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating metric");
                return StatusCode(500, new { message = "An error occurred while creating metric" });
            }
        }
    }
}

