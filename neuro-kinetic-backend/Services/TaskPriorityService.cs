using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace neuro_kinetic_backend.Services
{
    public enum TaskPriority
    {
        Critical = 100,
        High = 75,
        Medium = 50,
        Low = 25
    }
    
    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed,
        Blocked
    }
    
    public class TaskItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public TaskPriority Priority { get; set; }
        public List<string> Dependencies { get; set; } = new();
        public List<string> Blocking { get; set; } = new();
        public int EstimatedTime { get; set; } // in hours
        public TaskStatus Status { get; set; } = TaskStatus.Pending;
        public string? Assignee { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public double CalculatedPriority { get; set; }
    }
    
    public interface ITaskPriorityService
    {
        Task<List<TaskItem>> GetTasksAsync();
        Task<TaskItem?> GetTaskAsync(string id);
        Task<TaskItem> CreateTaskAsync(TaskItem task);
        Task<TaskItem> UpdateTaskAsync(string id, TaskItem task);
        Task<List<TaskItem>> GetExecutionOrderAsync();
        Task<TaskItem?> GetNextTaskAsync();
        Task<double> CalculatePriorityAsync(TaskItem task, List<TaskItem> allTasks);
        Task<bool> CanExecuteAsync(TaskItem task, List<TaskItem> allTasks);
    }
    
    public class TaskPriorityService : ITaskPriorityService
    {
        private readonly List<TaskItem> _tasks;
        
        public TaskPriorityService()
        {
            _tasks = InitializeDefaultTasks();
        }
        
        public Task<List<TaskItem>> GetTasksAsync()
        {
            return Task.FromResult(_tasks);
        }
        
        public Task<TaskItem?> GetTaskAsync(string id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            return Task.FromResult<TaskItem?>(task);
        }
        
        public Task<TaskItem> CreateTaskAsync(TaskItem task)
        {
            _tasks.Add(task);
            return Task.FromResult(task);
        }
        
        public Task<TaskItem> UpdateTaskAsync(string id, TaskItem task)
        {
            var existingTask = _tasks.FirstOrDefault(t => t.Id == id);
            if (existingTask == null)
                throw new KeyNotFoundException($"Task with ID {id} not found");
            
            existingTask.Name = task.Name;
            existingTask.Priority = task.Priority;
            existingTask.Dependencies = task.Dependencies;
            existingTask.Blocking = task.Blocking;
            existingTask.EstimatedTime = task.EstimatedTime;
            existingTask.Status = task.Status;
            existingTask.Assignee = task.Assignee;
            existingTask.StartedAt = task.StartedAt;
            existingTask.CompletedAt = task.CompletedAt;
            
            return Task.FromResult(existingTask);
        }
        
        public async Task<List<TaskItem>> GetExecutionOrderAsync()
        {
            var tasks = await GetTasksAsync();
            
            // Calculate priority for all tasks
            foreach (var task in tasks)
            {
                task.CalculatedPriority = await CalculatePriorityAsync(task, tasks);
            }
            
            // Sort by calculated priority (descending), then by estimated time (ascending)
            return tasks
                .OrderByDescending(t => t.CalculatedPriority)
                .ThenBy(t => t.EstimatedTime)
                .ToList();
        }
        
        public async Task<TaskItem?> GetNextTaskAsync()
        {
            var sortedTasks = await GetExecutionOrderAsync();
            
            return sortedTasks.FirstOrDefault(task =>
                task.Status == TaskStatus.Pending &&
                CanExecuteAsync(task, sortedTasks).Result
            );
        }
        
        public Task<double> CalculatePriorityAsync(TaskItem task, List<TaskItem> allTasks)
        {
            double score = (int)task.Priority;
            
            // Dependency weight: +20 per task this blocks
            if (task.Blocking.Any())
            {
                score += 20 * task.Blocking.Count;
            }
            
            // Blocking factor: +30 per critical task this blocks
            var blockingCritical = task.Blocking
                .Where(id => allTasks.FirstOrDefault(t => t.Id == id)?.Priority == TaskPriority.Critical)
                .Count();
            
            if (blockingCritical > 0)
            {
                score += 30 * blockingCritical;
            }
            
            // Urgency factor: +10 if has many dependencies (more urgent to unblock others)
            if (task.Dependencies.Count > 2)
            {
                score += 10;
            }
            
            return Task.FromResult(score);
        }
        
        public Task<bool> CanExecuteAsync(TaskItem task, List<TaskItem> allTasks)
        {
            // Check if all dependencies are completed
            var dependencies = task.Dependencies
                .Select(id => allTasks.FirstOrDefault(t => t.Id == id))
                .Where(t => t != null)
                .ToList();
            
            if (!dependencies.Any())
                return Task.FromResult(true);
            
            var allCompleted = dependencies.All(dep => dep!.Status == TaskStatus.Completed);
            return Task.FromResult(allCompleted);
        }
        
        private List<TaskItem> InitializeDefaultTasks()
        {
            return new List<TaskItem>
            {
                new TaskItem
                {
                    Id = "task-001",
                    Name = "Performance Metrics Dashboard",
                    Priority = TaskPriority.Critical,
                    Dependencies = new List<string>(),
                    Blocking = new List<string> { "task-003" },
                    EstimatedTime = 5,
                    Status = TaskStatus.Pending,
                    Assignee = "Backend Team"
                },
                new TaskItem
                {
                    Id = "task-002",
                    Name = "Technology Page Content",
                    Priority = TaskPriority.Critical,
                    Dependencies = new List<string> { "task-001" },
                    Blocking = new List<string> { "task-003" },
                    EstimatedTime = 4,
                    Status = TaskStatus.Pending,
                    Assignee = "Full Stack"
                },
                new TaskItem
                {
                    Id = "task-003",
                    Name = "Real API Integration for Demo",
                    Priority = TaskPriority.Critical,
                    Dependencies = new List<string> { "task-001", "task-002" },
                    Blocking = new List<string>(),
                    EstimatedTime = 5,
                    Status = TaskStatus.Pending,
                    Assignee = "Full Stack"
                },
                new TaskItem
                {
                    Id = "task-004",
                    Name = "Cross-Validation Results Display",
                    Priority = TaskPriority.Critical,
                    Dependencies = new List<string>(),
                    Blocking = new List<string> { "task-001" },
                    EstimatedTime = 4,
                    Status = TaskStatus.Pending,
                    Assignee = "Full Stack"
                },
                new TaskItem
                {
                    Id = "task-005",
                    Name = "Clinical Use Page",
                    Priority = TaskPriority.High,
                    Dependencies = new List<string> { "task-002" },
                    Blocking = new List<string>(),
                    EstimatedTime = 4,
                    Status = TaskStatus.Pending,
                    Assignee = "Full Stack"
                },
                new TaskItem
                {
                    Id = "task-006",
                    Name = "Collaboration Page",
                    Priority = TaskPriority.High,
                    Dependencies = new List<string>(),
                    Blocking = new List<string>(),
                    EstimatedTime = 3,
                    Status = TaskStatus.Pending,
                    Assignee = "Full Stack"
                },
                new TaskItem
                {
                    Id = "task-007",
                    Name = "Dataset Information Display",
                    Priority = TaskPriority.High,
                    Dependencies = new List<string>(),
                    Blocking = new List<string>(),
                    EstimatedTime = 3,
                    Status = TaskStatus.Pending,
                    Assignee = "Full Stack"
                },
                new TaskItem
                {
                    Id = "task-008",
                    Name = "Voice & Gait Analysis Modules",
                    Priority = TaskPriority.High,
                    Dependencies = new List<string>(),
                    Blocking = new List<string>(),
                    EstimatedTime = 5,
                    Status = TaskStatus.Pending,
                    Assignee = "Full Stack"
                },
                new TaskItem
                {
                    Id = "task-009",
                    Name = "Educational Resources",
                    Priority = TaskPriority.Medium,
                    Dependencies = new List<string>(),
                    Blocking = new List<string>(),
                    EstimatedTime = 4,
                    Status = TaskStatus.Pending,
                    Assignee = "Content Team"
                },
                new TaskItem
                {
                    Id = "task-010",
                    Name = "D3.js Visualizations",
                    Priority = TaskPriority.Medium,
                    Dependencies = new List<string> { "task-001" },
                    Blocking = new List<string>(),
                    EstimatedTime = 5,
                    Status = TaskStatus.Pending,
                    Assignee = "Frontend Team"
                },
                new TaskItem
                {
                    Id = "task-011",
                    Name = "Three.js 3D Animations",
                    Priority = TaskPriority.Low,
                    Dependencies = new List<string>(),
                    Blocking = new List<string>(),
                    EstimatedTime = 6,
                    Status = TaskStatus.Pending,
                    Assignee = "Frontend Team"
                }
            };
        }
    }
}



