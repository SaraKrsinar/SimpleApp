using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleApp.Data;
using SimpleApp.DTOs;
using SimpleApp.Models;
using System.Security.Claims;

namespace SimpleApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid user ID.");
            }

            // Retrieve projects belonging to the authenticated user.
            var projects = await _context.Projects.Where(p => p.UserId == userId).ToListAsync();
            return Ok(projects); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
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

            return Ok(project); // Ensure Ok() is used for ActionResult<Project>
        }

        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid user ID.");
            }

            project.UserId = userId; // Assign the user ID from the token

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.ProjectId }, Ok(project)); // Ensure Ok() is used
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, UpdateProjectDto projectDto)
        {
            if (id != projectDto.ProjectId)
            {
                return BadRequest();
            }

            var existingProject = await _context.Projects.FindAsync(id);

            if (existingProject == null)
            {
                return NotFound();
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid user ID.");
            }

            // Authorization check: Project ownership
            if (existingProject.UserId != userId)
            {
                return Unauthorized("Project does not belong to user.");
            }

            existingProject.Name = projectDto.Name;
            existingProject.Description = projectDto.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
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

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }
    }
}