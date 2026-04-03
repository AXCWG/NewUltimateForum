using System.ComponentModel.DataAnnotations;
using AXExpansion.AXHelper.Extensions;
using Microsoft.EntityFrameworkCore;

namespace UltimateForum.Server.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(Username),  IsUnique = true)]
public partial record User
{
    public long Id { get; init;  }
    
    public required string Username { get; set; }

    public string? Password
    {
        get;
        set
        {
            if (string.IsNullOrWhiteSpace(value) && GlobalStatic.ApplicationConfiguration.GetConfiguredValueOrThrow("MustPassword"))
            {
                throw new ValidationException($"{nameof(Password)} is required in this configuration.)"); 
            }

            field = value?.ToSha256String(); 
        }
    }

    public string? Email
    {
        get;
        set
        {
            if (string.IsNullOrWhiteSpace(value) && GlobalStatic.ApplicationConfiguration.GetConfiguredValueOrThrow("MustEmail"))
            {
                throw new ValidationException($"{nameof(Email)} is required in this configuration."); 
            }
            field = value;
        }
    }
    public required DateTime CreatedAt { get; set; }
    public ICollection<Board>? IsModeratorOf { get; set; }
    public ICollection<Post>? PostsPosted { get; set; }
    public ICollection<Reply>? RepliesReplied { get; set; }
    public ICollection<Board>? IsOpOf { get; set; }
    public ICollection<Board>? IsCreatorOf { get; set; }
    public ICollection<Tag>? IsTagCreatorOf { get; set; }
}

public record UserBody
{
    public long Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }

    public static implicit operator UserBody?(User? user) =>
        user is null ? null : new()
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
        };
}