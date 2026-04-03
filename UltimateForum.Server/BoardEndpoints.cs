using Microsoft.EntityFrameworkCore;
using UltimateForum.Server.Models;

namespace UltimateForum.Server;

public static class BoardEndpoints
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapGetAllBoard()
        {
            app.MapGet("all-board", (HttpContext context, UltimateForumDbContext db) =>
            {
                return Results.Ok(db.Boards.OrderByDescending(o => o.CreatedAt).Select(i => (BoardBody)i).AsEnumerable()); 
            });
            
            return app; 
        }

        public IEndpointRouteBuilder MapGetBoardUser()
        {
            app.MapGet("board-creator", (HttpContext context, UltimateForumDbContext db, long id) =>
            {
                var res = (UserBody?)db.Boards.Include(i => i.Creator).FirstOrDefault(p => p.Id == id)?.Creator;
                if (res is null)
                {
                    return Results.NotFound(); 
                }
                return Results.Ok(res);
            });
            return app; 
        }
    }

    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapAllBoardEndpoints()
        {
            app.MapGroup("/api/v1/board/").MapGetAllBoard().MapGetBoardUser();
            return app; 
        }
    }
}