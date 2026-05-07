using Microsoft.EntityFrameworkCore;
using UltimateForum.Server.Models;

namespace UltimateForum.Server.Endpoints.Api;

class PostBoardPayload
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}
public static class BoardEndpoints
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetAllBoard()
        {
            app.MapGet("all-board", (HttpContext context, UltimateForumDbContext db) =>
            {
                return Results.Ok(db.Boards.OrderByDescending(o => o.CreatedAt).Select(i => (BoardBody)i).AsEnumerable()); 
            }).Produces(200, typeof(IEnumerable<BoardBody>), "application/json");
            
            return app; 
        }

        public IEndpointRouteBuilder MapGetBoardUser()
        {
            app.MapGet("get-board-creator", (HttpContext context, UltimateForumDbContext db, long id) =>
            {
                var res = (UserBody?)db.Boards.Include(i => i.Creator).FirstOrDefault(p => p.Id == id)?.Creator;
                if (res is null)
                {
                    return Results.NotFound(); 
                }
                return Results.Ok(res);
            }).Produces<UserBody>(200).Produces(404);
            return app; 
        }

        public IEndpointRouteBuilder MapPostBoard()
        {
            app.MapPost("post-board", (HttpContext context, UltimateForumDbContext db, PostBoardPayload payload) =>
            {
                var longId = context.Session.GetLong("uid"); 
                if(longId is null)
                {
                    return Results.BadRequest("It's not possible to create board without logging in. "); 
                }
                User ?user = db.Users.Find(longId);
                if(user is null)
                {
                    return Results.BadRequest("It's not possible to create board without logging in. "); 
                }
                db.Boards.Add(new Board
                {
                    Name = payload.Name,
                    Description = payload.Description,
                    Op = user,
                    Creator = user,
                });
                
                db.SaveChanges();
                return Results.Ok(); 
            }).Produces(400, typeof(string), "text/plain");
            return app; 
        }
        
    }

    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapAllBoardEndpoints()
        {
            app.MapGroup("board/").MapGetAllBoard().MapGetBoardUser();
            return app; 
        }
    }
}