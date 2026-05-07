using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using JetBrains.Annotations;

namespace UltimateForum.Server.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(Title))]
[Index(nameof(PosterId))]
[Index(nameof(BoardAssociatedId))]
public record Post
{
    public long Id { get; set; }
    [MinLength(5)]
    public required string Title { get;
        set
        {
            if (value.Length < 5)
            {
                throw new ValidationException("Title must be at least 5 characters long");
            }

            field = value; 
        } }
    public required string Content { get; set; }
    public ICollection<Tag>? Tags { get; set; }
    /// <summary>
    /// Nullable depends on config. 
    /// </summary>
    public User? Poster { get;
        set
        {
            
            if (value is null && !GlobalStatic.ApplicationConfiguration.GetConfiguredValueOrThrow("AllowAnonymousPost"))
            {
                throw new ValidationException("AllowAnonymousPost is not on whilst posting in anonymous. ");
            }
            field = value;
        } }

    public int? PosterId
    {
        get;
        set
        {
            if (value is null && !GlobalStatic.ApplicationConfiguration.GetConfiguredValueOrThrow("AllowAnonymousPost"))
            {
                throw new ValidationException("AllowAnonymousPost is not on whilst posting in anonymous. ");
            }

            field = value; 
        }
    }

    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Should not be null in any given moment except for the occasion that this field is set through BoardAssociatedId property. 
    /// </summary>
    public Board BoardAssociated { get; set; } = null!;
    /// <summary>
    /// Should not be null in any given moment except for the occasion that this field is set through BoardAssociated property. 
    /// </summary>
    public long BoardAssociatedId { get;  init;  }
    public IEnumerable<Reply>? Replies { get; set; }
    public static implicit operator PostBody(Post p) => new()
        {
            Id = p.Id,
            Title = p.Title,
            Content = p.Content,
            Poster =  p.Poster,
            Tags = p.Tags?.Select(i=>(TagBody)i).ToList(),
            CreatedAt = p.CreatedAt
        };
}

public record PostBody
{
    public long? Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public UserBody? Poster { get; set; }
    public DateTime? CreatedAt { get; set; }
    public ICollection<TagBody>? Tags { get; set; }
    public IEnumerable<ReplyBody>? Replies { get; set; }
}