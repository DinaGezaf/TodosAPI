namespace TodosApp.DAL;
public interface IUnitOfWork : IDisposable
{
    ITodoRepository TodoRepository { get; }

    void BeginTransaction();
    void Commit();
    void Rollback();
    Task<int> SaveAsync();
}
