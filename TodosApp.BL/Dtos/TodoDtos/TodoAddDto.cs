using System.ComponentModel.DataAnnotations;


public class TodoAddDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    public bool Completed { get; set; }


}
