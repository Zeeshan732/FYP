using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using neuro_kinetic_backend.DTOs.TestRecord;
using neuro_kinetic_backend.Services;

namespace neuro_kinetic_backend.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _service;
        private readonly ILogger<DashboardController> _logger;
        
        public DashboardController(
            IAdminDashboardService service,
            ILogger<DashboardController> logger)
        {
            _service = service;
            _logger = logger;
        }
        
        [HttpGet("analytics")]
        public async Task<ActionResult<AdminDashboardAnalyticsDto>> GetAnalytics()
        {
            try
            {
                var analytics = await _service.GetAnalyticsAsync();
                return Ok(analytics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving admin dashboard analytics");
                return StatusCode(500, new { message = "An error occurred while retrieving analytics" });
            }
        }
    }
}

