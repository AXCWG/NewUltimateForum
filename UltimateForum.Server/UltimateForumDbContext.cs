using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Server.Models;

namespace UltimateForum.Server;

public class UltimateForumDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public UltimateForumDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Post> Posts { get; set; }
    public virtual DbSet<Tag> Tags { get; set; }
    public virtual DbSet<Reply> Replies { get; set; }
    public virtual DbSet<Board> Boards { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        switch (_configuration["Db"])
        {
            case "sqlite":
                optionsBuilder.UseSqlite(_configuration["ConnectionString"]);
                break;
            case "mysql":
                optionsBuilder.UseMySql(_configuration["ConnectionString"], ServerVersion.AutoDetect(_configuration["ConnectionString"]));
                break; 
            default:
                throw new InvalidOperationException("No db type specified or not valid. "); 
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>().HasOne(i => i.Poster).WithMany(u => u.PostsPosted).HasForeignKey(k => k.Id)
            .OnDelete(DeleteBehavior.ClientSetNull);
        modelBuilder.Entity<Board>().HasOne(b=>b.Creator).WithMany(b=>b.IsCreatorOf).HasForeignKey(k=>k.CreatorId).OnDelete(DeleteBehavior.ClientSetNull);
        modelBuilder.Entity<Board>().HasOne(b=>b.Op).WithMany(u=>u.IsOpOf).HasForeignKey(k=>k.OpId).OnDelete(DeleteBehavior.ClientSetNull);
        modelBuilder.Entity<Board>().HasMany(b => b.Moderators).WithMany(u => u.IsModeratorOf);
        modelBuilder.Entity<Reply>().HasOne(r => r.Creator).WithMany(u => u.RepliesReplied)
            .HasForeignKey(k => k.CreatorId).OnDelete(DeleteBehavior.ClientSetNull);
        modelBuilder.Entity<Tag>().HasOne(t => t.Creator).WithMany(u=>u.IsTagCreatorOf).HasForeignKey(k=>k.CreatorId).OnDelete(DeleteBehavior.ClientSetNull);
        modelBuilder.Entity<Board>().HasMany(b => b.Tags).WithMany(t => t.BoardsUtilizing);
        modelBuilder.Entity<Post>().HasMany(p => p.Tags).WithMany(t => t.PostsUtilizing); 
    }
}