using Prana.ServiceGateway.Models;
using System.Security.Claims;

namespace Prana.ServiceGateway.Contracts
{
    public interface ITokenService
    {
        string CreateToken(UserDto userDto);
        UserDto GetUserDtoFromTokenClain(ClaimsIdentity claimIdentity);

        Task<bool> IsCurrentActiveToken();
        Task DeactivateCurrentAsync();
        Task<bool> IsActiveAsync(string token);
        Task DeactivateAsync(string token);
        string CreateTouchOtk(TouchTokenDto touchTokenDto);
    }
}