using Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Persistence
{
    public class CachedAuthRepository : IAuthRepository
    {
        private readonly IAuthRepository _innerRepo;
        private readonly IMemoryCache _cache;

        public CachedAuthRepository(IAuthRepository innerRepo, IMemoryCache cache)
        {
            _innerRepo = innerRepo;
            _cache = cache;
        }

        public Task<bool> ValidateUserCredentialsAsync(string username, string password)
        {
            // Segurança: Não cachear validação de credenciais
            return _innerRepo.ValidateUserCredentialsAsync(username, password);
        }

        public async Task<string?> GetUserRoleAsync(string username)
        {
            string key = $"role_{username}";
            return await _cache.GetOrCreateAsync(key, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return _innerRepo.GetUserRoleAsync(username);
            });
        }
    }
}