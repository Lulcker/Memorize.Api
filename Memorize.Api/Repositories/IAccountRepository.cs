using Memorize.Library.Request;
using Memorize.Library.Response;
using Microsoft.AspNetCore.Mvc;

namespace Memorize.Api.Repositories
{
    public interface IAccountRepository
    {
        Task<(bool, AuthResponse)> Register([FromBody] RegistrationRequest request);

        Task<(bool, AuthResponse)> Login(UserLoginRequest request);
    }
}
