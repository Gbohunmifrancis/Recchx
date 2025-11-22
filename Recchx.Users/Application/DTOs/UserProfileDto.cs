namespace Recchx.Users.Application.DTOs;

public class UserProfileDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? Summary { get; set; }
    public List<string> Skills { get; set; } = new();
    public string? Preferences { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

