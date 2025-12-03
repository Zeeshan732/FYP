using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace neuro_kinetic_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;
        private readonly ILogger<HealthController> _logger;
        
        public HealthController(HealthCheckService healthCheckService, ILogger<HealthController> logger)
        {
            _healthCheckService = healthCheckService;
            _logger = logger;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetHealth()
        {
            var healthStatus = await _healthCheckService.CheckHealthAsync();
            
            var response = new
            {
                status = healthStatus.Status.ToString(),
                totalDuration = healthStatus.TotalDuration.TotalMilliseconds,
                entries = healthStatus.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    duration = e.Value.Duration.TotalMilliseconds,
                    data = e.Value.Data,
                    exception = e.Value.Exception?.Message
                })
            };
            
            var statusCode = healthStatus.Status == HealthStatus.Healthy ? 200 : 503;
            return StatusCode(statusCode, response);
        }
        
        [HttpGet("ready")]
        [AllowAnonymous]
        public async Task<ActionResult> GetReady()
        {
            var healthStatus = await _healthCheckService.CheckHealthAsync();
            
            if (healthStatus.Status == HealthStatus.Healthy)
            {
                return Ok(new { status = "ready" });
            }
            
            return StatusCode(503, new { status = "not ready" });
        }
        
        [HttpGet("live")]
        [AllowAnonymous]
        public ActionResult GetLive()
        {
            return Ok(new { status = "alive" });
        }
    }
}



