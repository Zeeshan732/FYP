using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using neuro_kinetic_backend.DTOs.Common;
using neuro_kinetic_backend.DTOs.TestRecord;
using neuro_kinetic_backend.Services;

namespace neuro_kinetic_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestRecordsController : ControllerBase
    {
        private readonly IUserTestRecordService _service;
        private readonly ILogger<TestRecordsController> _logger;
        
        public TestRecordsController(
            IUserTestRecordService service,
            ILogger<TestRecordsController> logger)
        {
            _service = service;
            _logger = logger;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResult<UserTestRecordDto>>> GetTestRecords(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortOrder = "desc",
            [FromQuery] int? userId = null,
            [FromQuery] string? status = null,
            [FromQuery] string? testResult = null)
        {
            try
            {
                // If user is authenticated but not admin, filter by their own userId
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var isAdmin = User.IsInRole("Admin");
                
                if (!isAdmin && userIdClaim != null && !userId.HasValue)
                {
                    userId = int.Parse(userIdClaim.Value);
                }
                
                var query = new GetTestRecordsQuery
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SortBy = sortBy ?? "testDate",
                    SortOrder = sortOrder ?? "desc",
                    UserId = userId,
                    Status = status,
                    TestResult = testResult
                };
                
                var result = await _service.GetTestRecordsAsync(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving test records");
                return StatusCode(500, new { message = "An error occurred while retrieving test records" });
            }
        }
        
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<UserTestRecordDto>>> GetAllTestRecords()
        {
            try
            {
                // If user is authenticated but not admin, filter by their own userId
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var isAdmin = User.IsInRole("Admin");
                
                if (!isAdmin && userIdClaim != null)
                {
                    var userId = int.Parse(userIdClaim.Value);
                    var records = await _service.GetTestRecordsByUserIdAsync(userId);
                    return Ok(records);
                }
                
                var allRecords = await _service.GetAllTestRecordsAsync();
                return Ok(allRecords);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving test records");
                return StatusCode(500, new { message = "An error occurred while retrieving test records" });
            }
        }
        
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<UserTestRecordDto>> GetTestRecord(int id)
        {
            try
            {
                var record = await _service.GetTestRecordByIdAsync(id);
                if (record == null)
                    return NotFound(new { message = "Test record not found" });
                
                // Check if user has permission (own record or admin)
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var isAdmin = User.IsInRole("Admin");
                
                if (!isAdmin && userIdClaim != null && record.UserId != int.Parse(userIdClaim.Value))
                {
                    return Forbid("You don't have permission to access this test record");
                }
                
                return Ok(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving test record");
                return StatusCode(500, new { message = "An error occurred while retrieving test record" });
            }
        }
        
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<UserTestRecordDto>> CreateTestRecord([FromBody] CreateUserTestRecordDto dto)
        {
            try
            {
                // Set userId from authenticated user if not provided
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (!dto.UserId.HasValue && userIdClaim != null)
                {
                    dto.UserId = int.Parse(userIdClaim.Value);
                }
                
                // Set userName from authenticated user if not provided
                var emailClaim = User.FindFirst(ClaimTypes.Email);
                if (string.IsNullOrEmpty(dto.UserName) && emailClaim != null)
                {
                    dto.UserName = emailClaim.Value;
                }
                
                var createdRecord = await _service.CreateTestRecordAsync(dto);
                return CreatedAtAction(nameof(GetTestRecord), new { id = createdRecord.Id }, createdRecord);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating test record");
                return StatusCode(500, new { message = "An error occurred while creating test record" });
            }
        }
        
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<UserTestRecordDto>> UpdateTestRecord(int id, [FromBody] UpdateUserTestRecordDto dto)
        {
            try
            {
                var record = await _service.GetTestRecordByIdAsync(id);
                if (record == null)
                    return NotFound(new { message = "Test record not found" });
                
                // Check if user has permission (own record or admin)
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var isAdmin = User.IsInRole("Admin");
                
                if (!isAdmin && userIdClaim != null && record.UserId != int.Parse(userIdClaim.Value))
                {
                    return Forbid("You don't have permission to update this test record");
                }
                
                var updatedRecord = await _service.UpdateTestRecordAsync(id, dto);
                return Ok(updatedRecord);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating test record");
                return StatusCode(500, new { message = "An error occurred while updating test record" });
            }
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteTestRecord(int id)
        {
            try
            {
                await _service.DeleteTestRecordAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Test record not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting test record");
                return StatusCode(500, new { message = "An error occurred while deleting test record" });
            }
        }
    }
}

