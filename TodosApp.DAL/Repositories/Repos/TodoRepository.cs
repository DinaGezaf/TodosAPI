using Microsoft.EntityFrameworkCore;


namespace TodosApp.DAL;
public class TodoRepository : ITodoRepository
{
    private readonly ApplicationDbContext _context;

    public TodoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoItem>> GetAllTodosAsync()
    {
        return await _context.TodoItems.ToListAsync();
    }

    public async Task<TodoItem> GetTodoByIdAsync(int id)
    {
        return await _context.TodoItems.FirstOrDefaultAsync(todo => todo.Id == id);
    }

    public async Task<IEnumerable<TodoItem>> GetTodosByUserIdAsync(string userId)
    {
        return await _context.TodoItems.Where(todo => todo.User.Id == userId).ToListAsync();
    }

    public async Task AddTodoAsync(TodoItem todo)
    {
        _context.TodoItems.Add(todo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTodoAsync(TodoItem todo)
    {
        _context.TodoItems.Update(todo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTodoAsync(int id)
    {
        var todo = await _context.TodoItems.FindAsync(id);
        if (todo != null)
        {
            _context.TodoItems.Remove(todo);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<ApplicationUser> GetUserByIdAsync(string userId)
    {
        return await _context.Users.FindAsync(userId);
    }
    public IQueryable<TodoItem> GetTodosPaginated()
    {
         return  _context.Set<TodoItem>().AsQueryable();
    }
}

