namespace UltimateForum.Server.Endpoints.PageSchema;

public static class MapAll
{
     extension(IEndpointRouteBuilder app)
     {
           public IEndpointRouteBuilder MapAllPageSchema()
           {
               var group = app.MapGroup("/schema/v1").IndexSchema();
               return group;
           }
     }   
}