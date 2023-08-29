namespace TodosApp.DAL;

public interface ITodoRepository
{
    Task<IEnumerable<TodoItem>> GetAllTodosAsync();
    Task<TodoItem> GetTodoByIdAsync(int id);
    Task<ApplicationUser> GetUserByIdAsync(string userId);
    Task<IEnumerable<TodoItem>> GetTodosByUserIdAsync(string userId);
    IQueryable<TodoItem> GetTodosPaginated();
    Task AddTodoAsync(TodoItem todo);
    Task UpdateTodoAsync(TodoItem todo);
    Task DeleteTodoAsync(int id);
}
