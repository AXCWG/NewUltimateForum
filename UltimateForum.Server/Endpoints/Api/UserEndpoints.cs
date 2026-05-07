using AXExpansion.AXHelper.Extensions;
using UltimateForum.Server.Models;

namespace UltimateForum.Server.Endpoints.Api;
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

record UserInfoResponse(
    long Id,
    string Username,
    string? Email,
    DateTime CreatedAt);
public static class UserEndpoints
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder Login()
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
            }).WithDescription("Login, server responses with Http Result along with session cookie. ")
            .Produces(200).Produces(400, typeof(string), "text/plain");
            app.MapPut("change-password",
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

                }).WithDescription("Changes password. Server reads session cookie and old password in body. If matches, user's password'd be changed. ")
                .Produces(200).Produces(401);
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
                return TypedResults.Ok(new UserInfoResponse(u.Id, u.Username, u.Email, u.CreatedAt)); 

            }).WithDescription("Gets information of current logged in user. ").Produces<UserInfoResponse>();
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
            }).WithDescription("Registers a new user, then auto login. "); 
            
            return app; 
        }

        
    }

    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapAllUserEndpoints()
        {
            app.MapGroup("/user/").Login(); 
            return app; 
        }
    }
}
