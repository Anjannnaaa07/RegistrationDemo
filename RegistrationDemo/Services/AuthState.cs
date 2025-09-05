using RegistrationDemo.Models;

namespace RegistrationDemo.Services
{
    public class AuthState : IAuthState
    {
        public UserDto? _currentUser;

        public bool IsLoggedIn()
        {
            return _currentUser != null;
        }

        public void Login(UserDto user)
        {
            _currentUser = user;
        }

        public void Logout()
        {
            _currentUser = null;
        }

        public UserDto? CurrentUser => _currentUser;
    }
}
