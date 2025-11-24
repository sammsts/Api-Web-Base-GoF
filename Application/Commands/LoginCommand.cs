using Application.Interfaces;

namespace Application.Commands
{
    public class LoginCommand
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandHandler
    {
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<object> Handle(LoginCommand command)
        {
            var token = await _authService.GenerateTokenAsync(command.Username, command.Password);
            return new { AccessToken = token, RefreshToken = Guid.NewGuid() };
        }
    }
}