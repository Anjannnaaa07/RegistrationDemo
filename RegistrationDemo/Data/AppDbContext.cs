using Microsoft.EntityFrameworkCore;
using RegistrationDemo.Models;
namespace RegistrationDemo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<UserDto> Users { get; set; }
    }
}
