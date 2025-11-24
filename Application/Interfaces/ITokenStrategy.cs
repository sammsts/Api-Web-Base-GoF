using System.Security.Claims;

namespace Application.Interfaces
{
    public interface ITokenStrategy
    {
        string GenerateToken(IEnumerable<Claim> claims);
    }
}