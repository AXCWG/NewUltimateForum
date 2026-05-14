using AXExpansion;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Server.Models;

namespace UltimateForum.Server.Endpoints.PageSchema;


public static class PageSchema
{
    record BoardPageSchema(UserBody? Creator, UserBody? Op, IEnumerable<UserBody>? Moderator, IEnumerable<PostBody> Posts, UserBody? UserBody);
    record IndexPageSchema(ICollection<BoardBody> Boards, UserBody? User);
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder IndexSchema()
        {
            app.MapGet("/index", (HttpContext context, UltimateForumDbContext dbContext) =>
            {
                var resp = new IndexPageSchema(dbContext.Boards.Include(p => p.Op).Include(p => p.Posts).ThenInclude(p=>p.Replies)
                    .AsNoTracking()
                    .AsEnumerable()
                    .Select(b =>
                    {
                       if (b.Op is not null)
                       {
                           b.Op = new()
                           {
                               Id = b.Op.Id,
                               Username = b.Op.Username,
                               CreatedAt = b.Op.CreatedAt,
                           };
                       }
                       return b;
                    }).Select(b =>
                    {
                        b.Posts = b.Posts.Select(p => new Post
                        {
                            Id = p.Id,
                            Title = p.Title,
                            Content = null!
                        }).ToList();
                        return b;
                    }).Select(i=>(BoardBody)i).ToList(), dbContext.Users.Find(context.Session.GetLong("uid")));
                return Results.Ok(resp);
            }).Produces<IndexPageSchema>();
            return app; 
        }

        public IEndpointRouteBuilder BoardSchema()
        {
            app.MapGet("/board/{id:long}", (long id, UltimateForumDbContext dbContext, HttpContext httpContext) =>
            {
                var board = dbContext.Boards
                    .Include(b=>b.Moderators)
                    .Include(b=>b.Op)
                    .Include(b=>b.Creator)
                    .Include(b=>b.Posts)
                    .AsNoTracking()
                    .FirstOrDefault(b => b.Id == id);
                if (board is null)
                    return Results.NotFound("Unknown board");
                return Results.Ok(new BoardPageSchema( board.Creator, board.Op, board.Moderators.Select(i=>(UserBody)i),board.Posts.Select(i=>(PostBody)i), dbContext.Users.Find(httpContext.Session.GetLong("uid"))));
            }).Produces<BoardPageSchema>();
            return app; 
        }
    }
}