using Microsoft.EntityFrameworkCore;

namespace UltimateForum.Server.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(Name))]
public record Tag
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public User? Creator { get; set; }
    public long? CreatorId { get; set; }
    public ICollection<Board>? BoardsUtilizing { get; set; }
    public ICollection<Post>? PostsUtilizing { get; set; }

    public static implicit operator TagBody(Tag t) => new()
        {
            Id = t.Id,
            Name = t.Name
        };
    
}

public record TagBody
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    
}