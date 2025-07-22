using Microsoft.AspNetCore.Identity;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Application.Services;

public class LoginService : ILoginService
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    public LoginService(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;

    }
    public async Task<SignInResult> LoginAsync(string usernameOrEmail, string password, bool rememberMe)
    {
        var result = await _signInManager.PasswordSignInAsync(usernameOrEmail, password, rememberMe, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(usernameOrEmail);
            if (user != null)
            {
                result = await _signInManager.PasswordSignInAsync(
                    user,
                    password,
                    rememberMe,
                    lockoutOnFailure: true
                );
            }
        }
        return result;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
