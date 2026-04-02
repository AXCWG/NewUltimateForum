using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace UltimateForum.Server.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(Title))]
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
}