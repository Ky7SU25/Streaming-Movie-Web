using Microsoft.AspNetCore.Identity;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Application.Interfaces;

public interface IResetPasswordService
{
    Task<string> GenerateResetPasswordTokenAsync(string email);
    Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword);
}
