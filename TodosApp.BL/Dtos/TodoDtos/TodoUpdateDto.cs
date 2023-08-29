using System.ComponentModel.DataAnnotations;

public class TodoUpdateDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    public bool Completed { get; set; } = true;

}
