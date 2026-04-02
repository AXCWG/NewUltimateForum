using AXExpansion.AXHelper.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Server;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<UltimateForumDbContext>();
builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromMinutes(30);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
    o.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});
const string defaultCors = "default";
builder.Services.AddCors(o =>
{
    o.AddPolicy(defaultCors, p =>
    {
        p.WithOrigins("http://localhost:3000").WithHeaders("Content-Type").AllowCredentials(); 
    });
});

var app = builder.Build();

GlobalStatic.ApplicationConfiguration = app.Configuration;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(o=>o.SwaggerEndpoint("/openapi/v1.json", "api")); 
    
}
app.UseCors(defaultCors);

app.UseSession();
using (var serviceScope = app.Services.CreateScope())
{
    var ultimateForumDbContext = serviceScope.ServiceProvider.GetService<UltimateForumDbContext>() ?? throw new Exception();
    ultimateForumDbContext.Database.Migrate();
    if (!ultimateForumDbContext.Users.Any(i=>i.Id == 1 || i.Username == "admin"))
    {
        ultimateForumDbContext.Users.Add(new()
        {
            Username = "admin",
            Password = app.Configuration["AdminDefaultPassword"],
            CreatedAt = DateTime.Now,
        });
        ultimateForumDbContext.SaveChanges();
    }
}
app.UseHttpsRedirection();

app.MapAllUserEndpoints();

app.Run();

