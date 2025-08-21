using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RegistrationDemo.Components;
using RegistrationDemo.Models;
using System.Net.NetworkInformation;
using System.Reflection.PortableExecutable;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient("ServerAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7196");
});

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ServerAPI"));



var users = new List<UserDto>();

var app = builder.Build();

app.MapGet("/api/users", () =>
{
    return Results.Ok(users);
});

app.MapPost("/api/register", (RegisterRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Username) ||
    string.IsNullOrWhiteSpace(request.Email) ||
    string.IsNullOrWhiteSpace(request.Password))
    {
        return Results.BadRequest(new { error = "All fields are required." });
    }

    if (users.Any(u => u.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase)))
    {
        return Results.BadRequest(new { error = "Username already exists." });
    }

    var user = new UserDto
    {
        Username = request.Username,
        Email = request.Email,
        RegisteredAtUtc = DateTime.UtcNow
    };
    users.Add(user);
    return Results.Created($"/api/users/{user.Id}", user);
});


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

var logger = app.Logger;
logger.LogInformation(" Application started!");

app.Run();
