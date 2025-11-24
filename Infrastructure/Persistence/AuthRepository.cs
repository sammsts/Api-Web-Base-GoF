using Application.Interfaces;

namespace Infrastructure.Persistence
{
    public class AuthRepository : IAuthRepository
    {
        // Simulated user data (this could be injected with a DbContext in a real application)
        private readonly Dictionary<string, (string Password, string Role)> _users = new()
        {
            { "admin", ("senha123", "Admin") },
            { "user", ("senha456", "User") }
        };

        public Task<bool> ValidateUserCredentialsAsync(string username, string password)
        {
            if (_users.TryGetValue(username, out var userInfo))
            {
                return Task.FromResult(userInfo.Password == password);
            }

            return Task.FromResult(false);
        }

        public Task<string?> GetUserRoleAsync(string username)
        {
            if (_users.TryGetValue(username, out var userInfo))
            {
                return Task.FromResult<string?>(userInfo.Role);
            }

            return Task.FromResult<string?>(null);
        }
    }
}
