using Recchx.SharedKernel.Entities;

namespace Recchx.Users.Domain.Entities;

public class UserProfile : BaseEntity
{
    public Guid UserId { get; private set; }
    public string? Summary { get; private set; }
    public List<string> Skills { get; private set; } = new();
    public string? Preferences { get; private set; } // JSON string
    public float[]? Embedding { get; private set; }
    
    public User User { get; private set; } = null!;

    private UserProfile() { } // EF Core

    public UserProfile(Guid userId, string? summary, List<string>? skills, string? preferences)
    {
        UserId = userId;
        Summary = summary;
        Skills = skills ?? new List<string>();
        Preferences = preferences;
    }

    public void UpdateProfile(string? summary, List<string>? skills, string? preferences)
    {
        Summary = summary;
        Skills = skills ?? new List<string>();
        Preferences = preferences;
        UpdateTimestamp();
    }

    public void SetEmbedding(float[] embedding)
    {
        Embedding = embedding;
        UpdateTimestamp();
    }
}

