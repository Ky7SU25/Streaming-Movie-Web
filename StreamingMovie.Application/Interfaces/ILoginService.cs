using Microsoft.AspNetCore.Identity;

namespace StreamingMovie.Application.Interfaces;

public interface ILoginService
{
    Task<SignInResult> LoginAsync(string usernameOrEmail, string password, bool rememberMe);
    Task LogoutAsync();
}
