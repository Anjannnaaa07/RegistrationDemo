using RegistrationDemo.Models;
using RegistrationDemo.Services;
using Xunit;

namespace RegistrationDemoTests.Services
{
    public class AuthStateTests
    {
        [Fact]
        public void IsLoggedIn_ShouldReturnFalse_WhenNoUserLoggedIn()
        {
            var authState = new AuthState();
            var result = authState.IsLoggedIn();
            Assert.False(result);
        }

        [Fact]
        public void IsLoggedIn_ShouldReturnTrue_WhenUserLoggedIn()
        {
            var authState = new AuthState();
            var user = new UserDto { Username = "TestUser" };
            authState.Login(user);
            var result = authState.IsLoggedIn();
            Assert.True(result);
        }

        [Fact]
        public void Login_ShouldSetCurrentUser()
        {
            var authState = new AuthState();
            var user = new UserDto { Username = "TestUser" };

            authState.Login(user);

            Assert.Equal(user, authState.CurrentUser);
            Assert.Equal("TestUser", authState.CurrentUser.Username);
        }
        [Theory]
        [InlineData("Alice")]
        [InlineData("Bob")]
        public void Login_ShouldSetUsername(string username)
        {
            var authState = new AuthState();
            var user = new UserDto { Username = username };

            authState.Login(user);

            Assert.Equal(username, authState.CurrentUser.Username);
        }


        [Fact]
        public void CurrentUser_ShouldBeNull_WhenNoUserLoggedIn()
        {
            var authState = new AuthState();
            var user = authState.CurrentUser;
            Assert.Null(user);
        }
    }
}
