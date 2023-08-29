
using System.ComponentModel.DataAnnotations;

public class TodoReadDto
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool Completed { get; set; }

}
