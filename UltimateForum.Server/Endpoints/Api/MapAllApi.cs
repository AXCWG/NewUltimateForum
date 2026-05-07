using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace UltimateForum.Server.Endpoints.Api;

public static class MapAllApi
{
    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapAllApiEndpoints()
        {
            app.MapGroup("/api/v1/").MapAllBoardEndpoints()
                .MapAllPostEndpoints()
                .MapAllUserEndpoints();
            return app; 
        }
    }
}