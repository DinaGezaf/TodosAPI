using Microsoft.EntityFrameworkCore.Storage;

namespace TodosApp.DAL;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction _transaction;
    public ITodoRepository TodoRepository { get; }

    public UnitOfWork(ApplicationDbContext context, ITodoRepository todoRepository)
    {
        _context = context;
        TodoRepository = todoRepository;
    }


    public void BeginTransaction()
    {
        _transaction = _context.Database.BeginTransaction();
    }

    public void Commit()
    {
        _transaction?.Commit();
    }

    public void Rollback()
    {
        _transaction?.Rollback();
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}

