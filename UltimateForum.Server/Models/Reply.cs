using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace UltimateForum.Server.Models;

[PrimaryKey(nameof(Id))]
public record Reply
{
    public long Id { get; set; }
    public required string Content { get; set; }

    public User? Creator
    {
        get;
        set
        {
            if (value is null && !GlobalStatic.ApplicationConfiguration.GetConfiguredValueOrThrow("AllowAnonymousReply"))
            {
                throw new ValidationException("AllowAnonymousReply is not on whilst replying in anonymous");
            }

            field = value; 
        }
    }

    public long? CreatorId
    {
        get;
        set
        {
            
            if (value is null && !GlobalStatic.ApplicationConfiguration.GetConfiguredValueOrThrow("AllowAnonymousReply"))
            {
                throw new ValidationException("AllowAnonymousReply is not on whilst replying in anonymous");
            }

            field = value;
        }
    }

    public required DateTime RepliedAt { get; set; }
    
}