using Microsoft.AspNetCore.Identity;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Application.Interfaces;

public interface IRegisterService
{
    Task <IdentityResult> RegisterAsync(User user, string password);
    Task<string> GenerateEmailConfirmationTokenAsync(User user);
    Task<IdentityResult> EmailConfirmTokenAsync(string userId, string code);
}
