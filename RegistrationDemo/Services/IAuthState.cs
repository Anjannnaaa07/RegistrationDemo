using RegistrationDemo.Models;

namespace RegistrationDemo.Services
{
    public interface IAuthState
    {
        bool IsLoggedIn();
        void Login(UserDto user);
        void Logout();
        UserDto? CurrentUser { get; }
    }
}
