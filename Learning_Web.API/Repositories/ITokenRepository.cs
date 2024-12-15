using Microsoft.AspNetCore.Identity;

namespace Learning_Web.API.Repositories
{
    public interface ITokenRepository
    {
        Task<string> GenerateToken(IdentityUser user, List<string> roles);

    }
}
