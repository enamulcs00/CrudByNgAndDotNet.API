using CrudByNgAndDotNet.API.Models.model;
using Microsoft.AspNetCore.Identity;

namespace CrudByNgAndDotNet.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(RegisterUser user, List<string> roles);
        string CreateRefreshToken();
    }
}
