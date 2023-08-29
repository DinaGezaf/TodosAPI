using System.ComponentModel.DataAnnotations;


  public interface ITodoService
{
    Task<IEnumerable<TodoReadDto>> GetAllTodosAsync();
    Task<TodoReadDto> GetTodoByIdAsync(int id, string userId);
    Task<List<ValidationResult>?> CreateTodoAsync(TodoAddDto todoAddDto, string userId);
    Task<List<ValidationResult>?> UpdateTodoAsync(int id, TodoUpdateDto todoUpdateDto, string userId);
    Task<bool> DeleteTodoAsync(int id, string userId);
    Task<IEnumerable<TodoReadDto>> GetTodosByUserIdAsync(string userId);

}

