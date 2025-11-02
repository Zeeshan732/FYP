using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using neuro_kinetic_backend.Services;
using TaskStatus = neuro_kinetic_backend.Services.TaskStatus;

namespace neuro_kinetic_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class TaskManagementController : ControllerBase
    {
        private readonly ITaskPriorityService _taskService;
        private readonly ILogger<TaskManagementController> _logger;
        
        public TaskManagementController(
            ITaskPriorityService taskService,
            ILogger<TaskManagementController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<ActionResult> GetAllTasks()
        {
            try
            {
                var tasks = await _taskService.GetTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tasks");
                return StatusCode(500, new { message = "An error occurred while retrieving tasks" });
            }
        }
        
        [HttpGet("execution-order")]
        public async Task<ActionResult> GetExecutionOrder()
        {
            try
            {
                var tasks = await _taskService.GetExecutionOrderAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating execution order");
                return StatusCode(500, new { message = "An error occurred while calculating execution order" });
            }
        }
        
        [HttpGet("next")]
        public async Task<ActionResult> GetNextTask()
        {
            try
            {
                var nextTask = await _taskService.GetNextTaskAsync();
                if (nextTask == null)
                    return NotFound(new { message = "No available tasks to execute" });
                
                return Ok(nextTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting next task");
                return StatusCode(500, new { message = "An error occurred while getting next task" });
            }
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult> GetTask(string id)
        {
            try
            {
                var task = await _taskService.GetTaskAsync(id);
                if (task == null)
                    return NotFound(new { message = $"Task with ID {id} not found" });
                
                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving task");
                return StatusCode(500, new { message = "An error occurred while retrieving task" });
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> CreateTask([FromBody] TaskItem task)
        {
            try
            {
                var createdTask = await _taskService.CreateTaskAsync(task);
                return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task");
                return StatusCode(500, new { message = "An error occurred while creating task" });
            }
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTask(string id, [FromBody] TaskItem task)
        {
            try
            {
                var updatedTask = await _taskService.UpdateTaskAsync(id, task);
                return Ok(updatedTask);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task");
                return StatusCode(500, new { message = "An error occurred while updating task" });
            }
        }
        
        [HttpPost("{id}/start")]
        public async Task<ActionResult> StartTask(string id)
        {
            try
            {
                var task = await _taskService.GetTaskAsync(id);
                if (task == null)
                    return NotFound(new { message = $"Task with ID {id} not found" });
                
                if (task.Status == TaskStatus.Completed)
                    return BadRequest(new { message = "Cannot start a completed task" });
                
                if (!await _taskService.CanExecuteAsync(task, await _taskService.GetTasksAsync()))
                    return BadRequest(new { message = "Task dependencies are not completed" });
                
                task.Status = TaskStatus.InProgress;
                task.StartedAt = DateTime.UtcNow;
                
                var updatedTask = await _taskService.UpdateTaskAsync(id, task);
                return Ok(updatedTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting task");
                return StatusCode(500, new { message = "An error occurred while starting task" });
            }
        }
        
        [HttpPost("{id}/complete")]
        public async Task<ActionResult> CompleteTask(string id)
        {
            try
            {
                var task = await _taskService.GetTaskAsync(id);
                if (task == null)
                    return NotFound(new { message = $"Task with ID {id} not found" });
                
                task.Status = TaskStatus.Completed;
                task.CompletedAt = DateTime.UtcNow;
                
                var updatedTask = await _taskService.UpdateTaskAsync(id, task);
                return Ok(updatedTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing task");
                return StatusCode(500, new { message = "An error occurred while completing task" });
            }
        }
        
        [HttpGet("progress")]
        public async Task<ActionResult> GetProgress()
        {
            try
            {
                var tasks = await _taskService.GetTasksAsync();
                
                var total = tasks.Count;
                var completed = tasks.Count(t => t.Status == TaskStatus.Completed);
                var inProgress = tasks.Count(t => t.Status == TaskStatus.InProgress);
                var pending = tasks.Count(t => t.Status == TaskStatus.Pending);
                var blocked = tasks.Count(t => t.Status == TaskStatus.Blocked);
                
                var byPriority = tasks
                    .GroupBy(t => t.Priority)
                    .Select(g => new
                    {
                        priority = g.Key.ToString(),
                        total = g.Count(),
                        completed = g.Count(t => t.Status == TaskStatus.Completed),
                        percentage = (double)g.Count(t => t.Status == TaskStatus.Completed) / g.Count() * 100
                    })
                    .ToList();
                
                return Ok(new
                {
                    overall = new
                    {
                        total,
                        completed,
                        inProgress,
                        pending,
                        blocked,
                        completionPercentage = (double)completed / total * 100
                    },
                    byPriority
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting progress");
                return StatusCode(500, new { message = "An error occurred while getting progress" });
            }
        }
    }
}

