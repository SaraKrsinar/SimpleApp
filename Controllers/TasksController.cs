using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskModel = SimpleApp.Models.Task; // Create an alias
using Microsoft.EntityFrameworkCore;
using SimpleApp.Data;
using System.Security.Claims;
using SimpleApp.DTOs;

namespace SimpleApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks(int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound("Project not found.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid user ID.");
            }

            // Authorization check: Project ownership
            if (project.UserId != userId)
            {
                return Unauthorized("Project does not belong to user.");
            }

            var tasks = await _context.Tasks.Where(t => t.ProjectId == projectId).ToListAsync();

            var taskDtos = tasks.Select(task => new TaskDto
            {
                TaskId = task.TaskId,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                ProjectId = task.ProjectId
            });

            return Ok(taskDtos);
        }

        [HttpGet("task/{id}")]
        public async Task<ActionResult<TaskDto>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(task.ProjectId);

            if (project == null)
            {
                return NotFound("Project not found.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid user ID.");
            }

            // Authorization check: Project ownership
            if (project.UserId != userId)
            {
                return Unauthorized("Project does not belong to user.");
            }

            var taskDto = new TaskDto
            {
                TaskId = task.TaskId,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                ProjectId = task.ProjectId
            };

            return Ok(taskDto);
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> PostTask(CreateTaskDto taskDto)
        {
            var project = await _context.Projects.FindAsync(taskDto.ProjectId);
            if (project == null)
            {
                return BadRequest("Project does not exist.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid user ID.");
            }

            // Authorization check: Project ownership
            if (project.UserId != userId)
            {
                return Unauthorized("Project does not belong to user.");
            }

            var task = new TaskModel
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                DueDate = taskDto.DueDate,
                ProjectId = taskDto.ProjectId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var taskResponseDto = new TaskDto
            {
                TaskId = task.TaskId,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                ProjectId = task.ProjectId
            };

            return CreatedAtAction(nameof(GetTask), new { id = task.TaskId }, taskResponseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, UpdateTaskDto taskDto)
        {
            if (id != taskDto.TaskId)
            {
                return BadRequest();
            }

            var originalTask = await _context.Tasks.FindAsync(id);

            if (originalTask == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(taskDto.ProjectId);

            if (project == null)
            {
                return NotFound("Project not found.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid user ID.");
            }

            // Authorization check: Project ownership
            if (project.UserId != userId)
            {
                return Unauthorized("Project does not belong to user.");
            }

            originalTask.Title = taskDto.Title;
            originalTask.Description = taskDto.Description;
            originalTask.DueDate = taskDto.DueDate;
            originalTask.ProjectId = taskDto.ProjectId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(task.ProjectId);

            if (project == null)
            {
                return NotFound("Project not found.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid user ID.");
            }

            // Authorization check: Project ownership
            if (project.UserId != userId)
            {
                return Unauthorized("Project does not belong to user.");
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.TaskId == id);
        }
    }
}