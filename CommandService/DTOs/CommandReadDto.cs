using System.ComponentModel.DataAnnotations;

namespace CommandService.DTOs;

public class CommandReadDto
{
    [Required]
    [Key]
    public int Id { get; set; }

    [Required]
    public string HowTo { get; set; }

    [Required]
    public string CommandLine { get; set; }

    [Required]
    public int PlatformId { get; set; }
}