using Recchx.SharedKernel.Entities;

namespace Recchx.Prospects.Domain.Entities;

public class Company : BaseEntity
{
    public string Name { get; private set; }
    public string? Domain { get; private set; }
    public string? Industry { get; private set; }
    public string? Size { get; private set; }
    public string? Description { get; private set; }
    public float[]? Embedding { get; private set; }
    
    public List<Contact> Contacts { get; private set; } = new();

    private Company() { } // EF Core

    public Company(string name, string? domain, string? industry, string? size, string? description)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Domain = domain;
        Industry = industry;
        Size = size;
        Description = description;
    }

    public void Update(string name, string? domain, string? industry, string? size, string? description)
    {
        Name = name;
        Domain = domain;
        Industry = industry;
        Size = size;
        Description = description;
        UpdateTimestamp();
    }

    public void SetEmbedding(float[] embedding)
    {
        Embedding = embedding;
        UpdateTimestamp();
    }
}

