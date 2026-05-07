using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace UltimateForum.Server.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(RepliedUnderId))]
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
    public required Post RepliedUnder { get; set; }
    public long RepliedUnderId { get; set; }

    public required DateTime RepliedAt { get; set; }

    public static implicit operator ReplyBody?(Reply? r)
    {
        return r is null
            ? null
            : new()
            {
                Id = r.Id,
                Content = r.Content,
                Creator = r.Creator,
                RepliedAt = r.RepliedAt
            };
    }
    
}

public record ReplyBody
{
    public long Id { get; set; }

    public string? Content
    {
        get; set;
    }
    public UserBody? Creator { get; set; }
    public DateTime? RepliedAt { get; set; }
}