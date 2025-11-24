using Application.Handlers;
using Application.Interfaces;
using System.Security.Claims;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenStrategy _tokenStrategy;

        public AuthService(IAuthRepository authRepository, ITokenStrategy tokenStrategy)
        {
            _authRepository = authRepository;
            _tokenStrategy = tokenStrategy;
        }

        public async Task<string> GenerateTokenAsync(string username, string password)
        {
            // CHAIN OF RESPONSIBILITY: Configuração da cadeia
            var validationChain = new UserExistsHandler();
            validationChain.SetNext(new PasswordCheckHandler());

            // Execução da validação
            await validationChain.ValidateAsync(username, password, _authRepository);

            // Se passou, pega a role
            var role = await _authRepository.GetUserRoleAsync(username);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role ?? "User")
            };

            // STRATEGY: Geração do token
            return _tokenStrategy.GenerateToken(claims);
        }
    }
}