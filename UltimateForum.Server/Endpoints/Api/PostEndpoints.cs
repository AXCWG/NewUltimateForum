using UltimateForum.Server.Models;

namespace UltimateForum.Server.Endpoints.Api;

public class PostPostPayload
{
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required long Under { get; set; }
}
public static class PostEndpoints
{
    extension<T>(T app) where T: IEndpointRouteBuilder
    {
        public T MapPostPost()
        {
            app.MapPost("post-post", (HttpContext context, UltimateForumDbContext db, PostPostPayload payload) =>
            {
                var poster = db.Users.Find(context.Session.GetLong("uid"));
                db.Posts.Add(new()
                {
                    Title = payload.Title,
                    Content = payload.Content,
                    Poster = poster,
                    CreatedAt = DateTime.Now,
                    BoardAssociatedId = payload.Under
                });
                db.SaveChanges(); 
                return Results.Ok(); 
            });
            return app; 
        }

        public T MapGetPost()
        {
            app.MapGet("get-post-under", (HttpContext context, UltimateForumDbContext db, long boardId) =>
            {
                var posts = db.Posts.Where(i => i.BoardAssociatedId == boardId).ToList();
                return posts.Count is 0 ? Results.BadRequest() : Results.Ok(posts);
            }).Produces(400).Produces(200, typeof(List<Post>), "application/json");
            return app; 
        }
        
    }
    extension<T>(T app) where T:  IEndpointRouteBuilder
    {
        public T MapAllPostEndpoints()
        {
            app.MapGroup("post").MapPostPost().MapGetPost();
            return app; 
        }
    }
}