using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace UltimateForum.Server.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(Name), IsUnique = true)]
[Index(nameof(OpId))]
[Index(nameof(CreatorId))]
public record Board
{
    public long Id { get; init; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public ICollection<User>? Moderators { get; set; }

    public required User Op { get; set; }
    public long  OpId { get; init; }

    /// <summary>
    /// Shall be defaulted to Admin. 
    /// </summary>
    public required User Creator { get;
        set
        {
            if (value.Username != "admin" && value.Id != 1 && !GlobalStatic.ApplicationConfiguration.GetConfiguredValueOrThrow("AllowUserCreateBoard"))
            {
                throw new ValidationException("Creater is not Admin when AllowUserCreateBoard is false. "); 
            }

            field = value; 
        } }

    public long CreatorId
    {
        get;
        init; 
    }
    
    public DateTime CreatedAt { get; set; }
    public ICollection<Tag>? Tags { get; set; }
    public ICollection<Post> Posts { get; set; } = [];
    
    public static implicit operator BoardBody(Board board)
    {
        
        return new()
        {
            Id = board.Id,
            Name = board.Name,
            Description = board.Description,
            Posts = board.Posts?.Select(i=>(PostBody)i),
            Creator = board.Creator,
            Op = board.Op,
            Moderators = board.Moderators?.Select(i=>(UserBody)i!)
        };
        
    }
}

public record BoardBody
{
    public long Id { get; set; }
    public  string? Name { get; set; }
    public  string? Description { get; set; }
    public IEnumerable<PostBody>? Posts { get; set; }
    public UserBody? Creator { get; set; }
    public UserBody? Op { get; set; }
    public IEnumerable<UserBody>? Moderators { get; set; }
    
};
