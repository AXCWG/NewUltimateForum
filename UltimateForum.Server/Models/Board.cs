using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace UltimateForum.Server.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(Name), IsUnique = true)]
public record Board
{
    public long Id { get; init; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public ICollection<User>? Moderators { get; set; }

    public required User Op { get; set; }
    public required long OpId { get; set; }

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

    public required long CreatorId
    {
        get;
        set
        {
            if (value != 1 && !GlobalStatic.ApplicationConfiguration.GetConfiguredValueOrThrow("AllowUserCreateBoard"))
            {
                throw new ValidationException("Creater is not Admin when AllowUserCreateBoard is false. "); 
            }

            field = value; 
        }
    }
    
    public DateTime CreatedAt { get; set; }
    public ICollection<Tag>? Tags { get; set; }
}