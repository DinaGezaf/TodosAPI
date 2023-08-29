namespace TodosApp.DAL;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool Completed { get; set; } = true;
    public virtual ApplicationUser? User { get; set; }
}
