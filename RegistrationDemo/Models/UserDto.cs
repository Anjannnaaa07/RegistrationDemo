namespace RegistrationDemo.Models
{
    public class UserDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime RegisteredAtUtc { get; set; } = DateTime.UtcNow;
    }
}
