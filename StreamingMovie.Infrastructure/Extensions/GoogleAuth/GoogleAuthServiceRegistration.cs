using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamingMovie.Application.Interfaces.ExternalServices.Google;
using StreamingMovie.Infrastructure.ExternalServices.Auth;

namespace StreamingMovie.Infrastructure.Extensions.GoogleAuth;

public static class GoogleAuthServiceRegistration
{
    public static IServiceCollection AddGoogleAuthService(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        //var googleAuthSettings = new GoogleAuthSettings();
        //config.GetSection("Authentication:Google").Bind(googleAuthSettings);

        //services.AddAuthentication(
        //    options =>
        //    {
        //        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        //        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        //    })       
        //    .AddGoogle(options =>
        //    {
        //        options.ClientId = googleAuthSettings.ClientId;
        //        options.ClientSecret = googleAuthSettings.ClientSecret;
        //        options.CallbackPath = "/signin-google";
        //    });



        services.AddScoped<IGoogleAuthService, GoogleAuthService>();
        return services;
    }
}
