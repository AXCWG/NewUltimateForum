using System.Diagnostics;
using AXExpansion.AXHelper.Extensions;
using UltimateForum.Server.Models;

namespace UltimateForum.Server;
class LoginPayload
{  
    public required string Username { get; set; }
    public string? Password { get; set; }
}

class RegisterPayload
{
    public required string Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}

class ChangePasswordPayload
{
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
}
public static class UserEndpoints
{
    extension(RouteGroupBuilder app)
    {
        public RouteGroupBuilder Login()
        {
            
            app.MapPost("login", (ILogger<WebApplication> logger, HttpContext context, UltimateForumDbContext db,  LoginPayload payload) =>
            {
                #if DEBUG
                logger.LogInformation("Req/Login: {0};{1}", payload.Username, payload.Password);
                #endif
                var pwd = payload.Password?.ToSha256String();
                if (db.Users.FirstOrDefault(u =>
                        u.Username == payload.Username && u.Password == pwd) is {} u)
                {
                    context.Session.SetString("uid", u.Id.ToString());
                    return Results.Ok(); 
                    
                }
                return Results.BadRequest("Username or password is incorrect");
            });
            app.MapPost("change-password",
                (HttpContext context, UltimateForumDbContext db, ChangePasswordPayload payload) =>
                {
                    if (context.Session.GetLong("uid") is {} id)
                    {
                        if (db.Users.FirstOrDefault(o=>o.Id == id && o.Password == payload.OldPassword) is {} user)
                        {
                            user.Password = payload.NewPassword;
                            db.SaveChanges();
                            return Results.Ok(); 
                        }
                    }

                    return Results.Unauthorized(); 

                });
            app.MapGet("info", (HttpContext context, UltimateForumDbContext db) =>
            {
                var uidS = context.Session.GetString("uid");
                if (uidS is null)
                {
                    return Results.NotFound();
                }
                if (!db.Users.Any(i => i.Id == long.Parse(uidS)))
                {
                    return Results.NotFound();
                }

                var u = db.Users.Find(long.Parse(uidS))!;
                return Results.Ok(new
                {
                    u.Id,
                    u.Username,
                    u.Email,
                    u.CreatedAt
                }); 

            });
            app.MapPost("register", (HttpContext context, UltimateForumDbContext db, RegisterPayload payload) =>
            {
                var entity = new User
                {
                    Username = payload.Username,
                    Password = payload.Password,
                    Email = payload.Email,
                    CreatedAt = DateTime.Now,
                };
                db.Users.Add(entity);
                db.SaveChanges(); 
                context.Session.SetString("uid", entity.Id.ToString());
                return Results.Ok();
            }); 
            
            return app; 
        }

        
    }

    extension(WebApplication app)
    {
        public WebApplication MapAllUserEndpoints()
        {
            app.MapGroup("/api/v1/user/").Login(); 
            return app; 
        }
    }
}
