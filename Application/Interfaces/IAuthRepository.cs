namespace Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> ValidateUserCredentialsAsync(string username, string password);
        Task<string?> GetUserRoleAsync(string username);
    }
}
