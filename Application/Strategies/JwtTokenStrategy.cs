using Application.Interfaces;
using Domain.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Strategies
{
    // Padrão BUILDER para construir o token complexo
    public class JwtBuilder
    {
        private string _issuer;
        private string _audience;
        private List<Claim> _claims = new();
        private DateTime _expiry;
        private SigningCredentials _creds;

        public JwtBuilder WithIssuer(string issuer) { _issuer = issuer; return this; }
        public JwtBuilder WithAudience(string audience) { _audience = audience; return this; }
        public JwtBuilder WithClaims(IEnumerable<Claim> claims) { _claims.AddRange(claims); return this; }
        public JwtBuilder WithExpiry(int minutes) { _expiry = DateTime.UtcNow.AddMinutes(minutes); return this; }
        public JwtBuilder WithKey(string key)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
            _creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            return this;
        }

        public JwtSecurityToken Build()
        {
            return new JwtSecurityToken(_issuer, _audience, _claims, null, _expiry, _creds);
        }
    }

    // Padrão STRATEGY para a geração do token
    public class JwtTokenStrategy : ITokenStrategy
    {
        private readonly JwtOptions _options;

        public JwtTokenStrategy(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var token = new JwtBuilder()
                .WithIssuer(_options.Issuer)
                .WithAudience(_options.Audience)
                .WithClaims(claims)
                .WithExpiry(_options.AccessTokenExpiryMinutes)
                .WithKey(_options.Key)
                .Build();

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}