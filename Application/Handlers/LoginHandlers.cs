using Application.Interfaces;

namespace Application.Handlers
{
    public abstract class LoginHandler
    {
        protected LoginHandler _next;
        public LoginHandler SetNext(LoginHandler next) { _next = next; return next; }
        public abstract Task ValidateAsync(string username, string password, IAuthRepository repo);
    }

    public class UserExistsHandler : LoginHandler
    {
        public override async Task ValidateAsync(string username, string password, IAuthRepository repo)
        {
            // Verifica se o usuário existe (simulado aqui pegando a role)
            var role = await repo.GetUserRoleAsync(username);
            if (role == null)
                throw new UnauthorizedAccessException("Usuário não encontrado.");

            if (_next != null) await _next.ValidateAsync(username, password, repo);
        }
    }

    public class PasswordCheckHandler : LoginHandler
    {
        public override async Task ValidateAsync(string username, string password, IAuthRepository repo)
        {
            var isValid = await repo.ValidateUserCredentialsAsync(username, password);
            if (!isValid)
                throw new UnauthorizedAccessException("Senha incorreta.");

            if (_next != null) await _next.ValidateAsync(username, password, repo);
        }
    }
}