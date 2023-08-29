
using Microsoft.AspNetCore.Mvc;
using Shipping.Services.Dtos;

public interface IExternalTodoService
{
    Task<IEnumerable<TodoReadDto>> GetExternalTodosAsync();
    Task<PaginationResponse<TodoReadDto>> GetTodosPaginatedAsync([FromQuery] PaginationParameters paginationParameters);
}
