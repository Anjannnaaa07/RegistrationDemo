using Microsoft.AspNetCore.Mvc;
using RegistrationDemo.Data;
using RegistrationDemo.Models;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    private async Task<UserDto?> FindUserByUsername(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _context.Users.ToListAsync();
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { error = "All fields are required." });
        }

        var existingUser = await FindUserByUsername(request.Username);

        if (existingUser != null)
        {
            return BadRequest(new { error = "Username already exists." });
        }

        var user = new UserDto
        {
            Username = request.Username,
            Email = request.Email,
            Password = request.Password
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUserByUsername), new { username = user.Username }, user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password);

        if (user == null)
        {
            return Unauthorized(new { error = "Invalid username or password." });
        }

        return Ok(new UserDto { Username = user.Username, Email = user.Email });
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        var user = await FindUserByUsername(username);

        if (user == null)
        {
            return NotFound(new { error = "User not found." });
        }

        return Ok(user);
    }

    [HttpDelete("{username}")]
    public async Task<IActionResult> DeleteUser(string username)
    {
        var user = await FindUserByUsername(username);
        if (user == null)
            return NotFound();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
