using Microsoft.EntityFrameworkCore;
using UltimateForum.Server.Models;

namespace UltimateForum.Server.Endpoints.PageSchema;


public static class PageSchema
{
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
                        b.Op = new()
                        {
                            Id = b.OpId,
                            Username = b.Op.Username,
                            CreatedAt = b.Op.CreatedAt,
                        };
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
    }
}