using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using RegistrationDemo.Components;
using RegistrationDemo.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddHttpClient("ServerAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AppUrl"] ?? "https://localhost:7196");
});

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ServerAPI"));

var users = new List<UserDto>();

var app = builder.Build();

app.MapGet("/api/users", () =>
{
    return Results.Ok(users);
}).DisableAntiforgery();

app.MapPost("/api/register", ([FromBody]RegisterRequest request) =>
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
    };
    users.Add(user);
    return Results.Created($"/api/users/{user.Username}", user);
}).DisableAntiforgery();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
