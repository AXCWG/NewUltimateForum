using System.Text;
using Microsoft.EntityFrameworkCore;
using NSwag;
using NSwag.CodeGeneration.TypeScript;
using Scalar.AspNetCore;
using UltimateForum.Server;
using UltimateForum.Server.Endpoints;
using UltimateForum.Server.Endpoints.Api;
using UltimateForum.Server.Endpoints.PageSchema;
using UltimateForum.Server.Models;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<UltimateForumDbContext>();
builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromMinutes(30);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
    o.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});
const string defaultCors = "default";
builder.Services.AddCors(o =>
{
    o.AddPolicy(defaultCors, p =>
    {
        p.WithOrigins("http://localhost:3000").WithHeaders("Content-Type").AllowCredentials(); 
    });
});

var app = builder.Build();

GlobalStatic.ApplicationConfiguration = app.Configuration;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

}
app.UseCors(defaultCors);

app.UseSession();
using (var serviceScope = app.Services.CreateScope())
{
    var ultimateForumDbContext = serviceScope.ServiceProvider.GetService<UltimateForumDbContext>() ?? throw new Exception();
    ultimateForumDbContext.Database.Migrate();
    if (!ultimateForumDbContext.Users.Any(i=>i.Id == 1 || i.Username == "admin"))
    {
        ultimateForumDbContext.Users.Add(new()
        {
            Username = "admin",
            Password = app.Configuration["AdminDefaultPassword"],
            CreatedAt = DateTime.Now,
        });
        ultimateForumDbContext.SaveChanges();
    }

    #region Init
    {
        var admin = ultimateForumDbContext.Users.Find(1L)?? throw new InvalidOperationException("No admin?? How? ");
        
        if (!ultimateForumDbContext.Boards.Any())
        {
            ultimateForumDbContext.Boards.Add(new Board
            {
                Name = "Default Board",
                Description = "Default board created in initialization.",
                Op = admin ,
                Creator = admin
            });
            ultimateForumDbContext.SaveChanges();
        }

        var defBoard = ultimateForumDbContext.Boards.Find(1L) ??
                       throw new InvalidOperationException("No default board?? How? ");
        if (!ultimateForumDbContext.Posts.Any())
        {
            ultimateForumDbContext.Posts.Add(new Post
            {
                Title = "Welcome to Ultimate Forum!",
                Content = "This is the first post in the default board. Feel free to create new posts and discuss!",
                BoardAssociated = defBoard,
                Poster = admin,
                CreatedAt = DateTime.Now,
            });
            ultimateForumDbContext.SaveChanges();
        }

        var post = ultimateForumDbContext.Posts.Find(1L) ??
                   throw new InvalidOperationException("No default post?? how?");
        if (!ultimateForumDbContext.Replies.Any())
        {
            ultimateForumDbContext.Replies.Add(new Reply
            {
                Content = "Welcome :3",
                RepliedUnder = post,
                RepliedAt = DateTime.Now
            });
            ultimateForumDbContext.SaveChanges();
        }
    }
    #endregion
    
}
app.UseHttpsRedirection();
app.MapGet("/ping", () => Results.Ok("pong")).Produces(200, typeof(string), "text/plain");
app.MapAllSystemEndpoints();
app.MapAllApiEndpoints();
app.MapAllPageSchema();

app.Run();


