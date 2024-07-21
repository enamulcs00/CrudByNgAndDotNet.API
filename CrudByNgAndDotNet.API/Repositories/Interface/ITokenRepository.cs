using Microsoft.AspNetCore.Identity;

namespace CrudByNgAndDotNet.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
