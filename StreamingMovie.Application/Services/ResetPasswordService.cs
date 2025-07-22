using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;
using System.Text;

namespace StreamingMovie.Application.Services;

public class ResetPasswordService : IResetPasswordService
{
    private readonly UserManager<User> _userManager;
    public ResetPasswordService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    public async Task<string> GenerateResetPasswordTokenAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || ! await _userManager.IsEmailConfirmedAsync(user))
        {
            throw new ArgumentException("User not found.");
        }
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        return code;
    }

    public async Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            return IdentityResult.Failed(new IdentityError { Description = "Invalid user." });
        token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        return await _userManager.ResetPasswordAsync(user, token, newPassword);
    }
}
