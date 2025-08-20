using Microsoft.AspNetCore.Identity;

namespace blogExploraLatamAPI.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
