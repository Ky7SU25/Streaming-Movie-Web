using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;
using System;
using System.Text;

namespace StreamingMovie.Application.Services;

public class RegisterService : IRegisterService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    public RegisterService(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IdentityResult> EmailConfirmTokenAsync(string userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found.");
        }

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
        }
        return result;
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
    {
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
    }

    public async Task<IdentityResult> RegisterAsync(User user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }
}
