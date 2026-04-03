namespace UltimateForum.Server;

public static class SystemEndpoints
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapCounts()
        {
            app.MapGet("count", (UltimateForumDbContext db, HttpContext context) =>
            {
                if (context.Session.GetLong("uid") != 1)
                {
                    return Results.Unauthorized(); 
                }
                return Results.Ok(new
                {
                    BoardCount = db.Boards.Count(),
                    PostCount = db.Posts.Count(),
                    UserCount = db.Users.Count(),
                    ReplyCount = db.Replies.Count(),
                    TagCount = db.Tags.Count()
                });
            }); 
            return app; 
        }
    }

    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapAllSystemEndpoints()
        {
             var g = app.MapGroup("/api/v1/system");
             g.AddEndpointFilter(async (EndpointFilterInvocationContext context, EndpointFilterDelegate @delegate) =>
             {
                 var logger = context.HttpContext.RequestServices.GetService<ILogger<WebApplication>>();
                 
                 if (context.HttpContext.Session.GetLong("uid") != 1)
                 {
                     logger?.LogWarning("[{0}] unauthorized request at: {1}", context.HttpContext.Request.Host.Host, context.HttpContext.Request.Path);
                     return Results.Unauthorized();
                 }
                 return  await @delegate(context); 
             });
             g.MapCounts();
             return app; 
        }
    }
}