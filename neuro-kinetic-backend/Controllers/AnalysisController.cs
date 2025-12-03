using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using neuro_kinetic_backend.DTOs.Analysis;
using neuro_kinetic_backend.Services;

namespace neuro_kinetic_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalysisController : ControllerBase
    {
        private readonly IAnalysisService _analysisService;
        private readonly ILogger<AnalysisController> _logger;
        
        public AnalysisController(IAnalysisService analysisService, ILogger<AnalysisController> logger)
        {
            _analysisService = analysisService;
            _logger = logger;
        }
        
        [HttpPost("process")]
        [AllowAnonymous]
        public async Task<ActionResult<AnalysisResponse>> ProcessAnalysis([FromBody] AnalysisRequest request)
        {
            try
            {
                var response = await _analysisService.ProcessAnalysisAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing analysis");
                return StatusCode(500, new { message = "An error occurred while processing analysis" });
            }
        }
        
        [HttpGet("session/{sessionId}")]
        [AllowAnonymous]
        public async Task<ActionResult<AnalysisResponse>> GetBySessionId(string sessionId)
        {
            try
            {
                var response = await _analysisService.GetAnalysisBySessionIdAsync(sessionId);
                if (response == null)
                    return NotFound(new { message = "Analysis not found" });
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving analysis");
                return StatusCode(500, new { message = "An error occurred while retrieving analysis" });
            }
        }
        
        [HttpGet("recent")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AnalysisResponse>>> GetRecent([FromQuery] int count = 10)
        {
            try
            {
                var responses = await _analysisService.GetRecentAnalysesAsync(count);
                return Ok(responses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving recent analyses");
                return StatusCode(500, new { message = "An error occurred while retrieving recent analyses" });
            }
        }
    }
}



