using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.Services.Dtos;

namespace Todos_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiveToDoController : ControllerBase
    {
        private readonly IExternalTodoService _externalTodoService;

        public LiveToDoController( IExternalTodoService externalTodoService)
        {
            _externalTodoService = externalTodoService;
        }

        [HttpGet("externaltodos")]
        public async Task<ActionResult<IEnumerable<TodoReadDto>>> GetExternalTodos()
        {
            var externalTodos = await _externalTodoService.GetExternalTodosAsync();
            return Ok(externalTodos);
        }
        [HttpGet("paginated")]
        public async Task<ActionResult<PaginationResponse<TodoReadDto>>> GetTodosPaginated([FromQuery] PaginationParameters paginationParameters)
        {
            var paginatedTodos = await _externalTodoService.GetTodosPaginatedAsync(paginationParameters);
            return Ok(paginatedTodos);
        }
    }
}
