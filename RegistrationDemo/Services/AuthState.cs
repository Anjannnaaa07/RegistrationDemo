namespace RegistrationDemo.Services
{
    public class AuthState
    {
        public Models.UserDto? CurrentUser { get; set; }
        public bool IsLoggedIn => CurrentUser != null;
    }
}
