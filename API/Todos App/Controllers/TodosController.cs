using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shipping.Services.Dtos;
using System.Security.Claims;

namespace Shipping.API.Controllers
{
    [Route("api/Todos")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodosController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IEnumerable<TodoReadDto>> GetAllTodosAsync()
        {
            return await _todoService.GetAllTodosAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminAndUsers")]
        public async Task<ActionResult<TodoReadDto>> GetTodoByIdAsync(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var todo = await _todoService.GetTodoByIdAsync(id, userId);
            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }

        [HttpPost]
        [Authorize(Policy = "AdminAndUsers")]
        public async Task<IActionResult> CreateTodoAsync([FromBody] TodoAddDto todoAddDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var validationResults = await _todoService.CreateTodoAsync(todoAddDto,userId);
            if (validationResults?.Count == 0)
            {
                return Ok("Todo created successfully.");
            }
            return BadRequest(validationResults);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminAndUsers")]
        public async Task<IActionResult> UpdateTodoAsync(int id, [FromBody] TodoUpdateDto todoUpdateDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var validationResults = await _todoService.UpdateTodoAsync(id, todoUpdateDto,userId);
            if (validationResults?.Count == 0)
            {
                return Ok("Todo updated successfully.");
            }
            return BadRequest(validationResults);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminAndUsers")]
        public async Task<IActionResult> DeleteTodoAsync(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var deleted = await _todoService.DeleteTodoAsync(id,userId);
            if (deleted)
            {
                return Ok("Todo deleted successfully.");
            }
            return NotFound();
        }

        [HttpGet("user")]
        [Authorize(Policy = "AdminAndUsers")]
        public async Task<IEnumerable<TodoReadDto>> GetTodosByUserIdAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _todoService.GetTodosByUserIdAsync(userId);
        }

    }
}
