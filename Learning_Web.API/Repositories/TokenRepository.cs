using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Learning_Web.API.Exceptions;

namespace Learning_Web.API.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenRepository> _logger;

        public TokenRepository(IConfiguration configuration, ILogger<TokenRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> GenerateToken(IdentityUser user, List<string> roles)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email)
                };
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                // create key
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                // create credentials
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // create token
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials
                );

                return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
            }
            catch (Exception ex)
            {
                // Log the exception (logging mechanism not implemented in this example)
                _logger.LogError(ex, "An error occurred while generating the token.");
                throw new ApplicationException("An error occurred while generating the token.", ex);
            }
        }
    }
}
