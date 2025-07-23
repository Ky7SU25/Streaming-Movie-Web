using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Application.Interfaces.ExternalServices.Google;
using StreamingMovie.Application.Interfaces.ExternalServices.Mail;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Web.Views.Account.ViewModels;
using StreamingMovie.Web.Views.Home.Controllers;
using StreamingMovie.Web.Views.Shared.ViewModels;
using System.Diagnostics;

namespace StreamingMovie.Web.Views.Account.Controllers;

public class AccountController : Controller
{
    private readonly ILoginService _loginService;
    private readonly ILogger<AccountController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRegisterService _registerService;
    private readonly IMailService _mailService;
    private readonly IResetPasswordService _resetPasswordService;
    private readonly IGoogleAuthService _googleAuthService;

    public AccountController(ILoginService loginService, ILogger<AccountController> logger, IHttpContextAccessor httpContextAccessor, IRegisterService registerService, IMailService mailService, IResetPasswordService resetPasswordService, IGoogleAuthService googleAuthService)
    {
        _loginService = loginService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _registerService = registerService;
        _mailService = mailService;
        _resetPasswordService = resetPasswordService;
        _googleAuthService = googleAuthService;
    }

    // GET: /Account/Login
    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        var isLogin = _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        if (isLogin == true)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");
        var isLogin = _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        if ( isLogin == true)
        {
            return RedirectToAction("Index", "Home");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _loginService.LoginAsync(model.Email, model.Password, model.RememberMe);

        if (result.Succeeded)
        {
            _logger.LogInformation("User logged in successfully.");
            return RedirectToLocal(returnUrl);
        }
        if (result.IsLockedOut)
        {
            _logger.LogWarning("User account locked out for user {Email}.", model.Email);
            ModelState.AddModelError(string.Empty, "Your account has been locked out. Please try again later.");
            return View(model);
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            _logger.LogWarning("Invalid login attempt for user {Email}.", model.Email);
        }

        return View(model);
    }

    // GET: /Account/Register (Tạm thời để tạo user)
    [HttpGet]
    public IActionResult Register(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // POST: /Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        if (ModelState.IsValid)
        {
            var newUser = new User
            {
                FullName = model.UserName,
                Email = model.Email,
                UserName = model.Email, // Use email as username
            };
            var result = await _registerService.RegisterAsync(newUser, model.Password);

            if (result.Succeeded) 
            {
                _logger.LogInformation("User registered successfully.");
                var token = await _registerService.GenerateEmailConfirmationTokenAsync(newUser);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = newUser.Id,
                    code = token,
                    returnUrl = returnUrl
                }, protocol: Request.Scheme);

                var root = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
                var templatePath = Path.Combine(root, "StreamingMovie.Domain", "Common", "Templates", "ConfirmEmailTemplate.html");

                string htmlTemplate = await System.IO.File.ReadAllTextAsync(templatePath);

                string emailBody = htmlTemplate
                .Replace("{{ .ConfirmationURL }}", callbackUrl)
                .Replace("{{ .Email }}", model.Email);

                var mailContent = new MailContent
                {
                    To = model.Email,
                    Subject = "Confirm Email CineStream",
                    Body = emailBody
                };
                await _mailService.SendMailAsync(mailContent);
                TempData["RegisterSuccess"] = "Register successfully, please check your email and click in confirmation link to complete.";
                return RedirectToAction("Register"); 
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
        }
        return View(model);
    }
    // GET: /Account/ConfirmEmail
    public async Task<IActionResult> ConfirmEmail(string userId, string code, string returnUrl = null)
    {
        try
        {
            var result = await _registerService.EmailConfirmTokenAsync(userId, code);
        if (result.Succeeded)
        {
            _logger.LogInformation("User email confirmed successfully.");
            return View("ConfirmEmail", new ConfirmEmailViewModel
            {
                Title = "Email confirm successfully",
                Message = "Your email has been verrified. Redirecting...",
                RedirectUrl = returnUrl ?? Url.Action("Index", "Home")
            });
        }
        else
        {
            _logger.LogWarning("Email confirmation failed for user {UserId}.", userId);
            return View("ConfirmEmail", new ConfirmEmailViewModel
            {
                Title = "Email confirm",
                Message = "You has not confirmed your email. Contact with Adminitristor if you think this is an error.",
                RedirectUrl = Url.Action("Index", "Home")
            });
        }
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error confirming email for user {UserId}.", userId);
            return View("ConfirmEmail", new ConfirmEmailViewModel
            {
                Title = "Error",
                Message = "An error occurred while confirming your email. Please try again later.",
                RedirectUrl = Url.Action("Index", "Home")
            });
        }
    }

    [HttpGet]
    public IActionResult ForgotPassword(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model, string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        if (ModelState.IsValid)
        {
            string token;
            try
            {
                 token = await _resetPasswordService.GenerateResetPasswordTokenAsync(model.Email) ?? "";
            }
            catch( Exception e) 
            {
                _logger.LogError(e, "Error generating password reset token for email {Email}.", model.Email);
                ModelState.AddModelError(string.Empty, "Email not found or user hasn't confirmed email yet.");
                return View(model);
            }

            var callbackUrl = Url.Action("ResetPassword", "Account", new
            {
                email = model.Email,
                code = token,
                returnUrl = returnUrl
            }, protocol: Request.Scheme);


            var root = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            var templatePath = Path.Combine(root, "StreamingMovie.Domain", "Common", "Templates", "ResetPasswordTemplate.html");

            string htmlTemplate = await System.IO.File.ReadAllTextAsync(templatePath);

            string emailBody = htmlTemplate
                .Replace("{{ .ResetPasswordUrl }}", callbackUrl)
                .Replace("{{ .Email }}", model.Email);

            var mailContent = new MailContent
            {
                To = model.Email,
                Subject = "Reset Password CineStream",
                Body = emailBody
            };
            await _mailService.SendMailAsync(mailContent);
            TempData["ForgotPasswordSuccess"] = "Please check your email to reset your password.";
            return RedirectToAction("ForgotPassword");
        }
        return View(model);
    }

    // GET: /Account/ResetPassword
    [HttpGet]
    public IActionResult ResetPassword(string email, string code, string returnUrl = null)
    {
        if (email == null || code == null)
        {
            return RedirectToAction("Error");
        }

        var model = new ResetPasswordViewModel
        {
            Email = email,
            Code = code
        };

        ViewData["ReturnUrl"] = returnUrl;
        return View(model);
    }

    // POST: /Account/ResetPassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model, string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _resetPasswordService.ResetPasswordAsync(model.Email, model.Code, model.Password);
        if (result.Succeeded)
        {
            TempData["ResetPasswordSuccess"] = "Password reset successfully. Please login with your new password.";
            return RedirectToAction("Login");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public   IActionResult ExternalLogin(string provider, string returnUrl = null)
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, provider); // provider = "Google"
    }

    [HttpGet]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
    {
        returnUrl ??= Url.Content("~/");

        if (remoteError != null)
        {
            _logger.LogError("Google login error: {Error}", remoteError);
            TempData["LoginError"] = $"Error from external provider: {remoteError}";
            return RedirectToAction(nameof(Login));
        }

        var externalResult = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
        if (!externalResult.Succeeded)
        {
            _logger.LogWarning("External auth cookie not found.");
        }
        else
        {
            _logger.LogInformation("External auth cookie found.");
        }

        var info = await _googleAuthService.GetExternalLoginInfoAsync();
        if (info == null)
        {
            _logger.LogWarning("Error loading external login info.");
            return RedirectToAction(nameof(Login));
        }

        var signInResult = await _googleAuthService.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey);

        if (signInResult.Succeeded)
        {
            _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
            return RedirectToLocal(returnUrl);
        }

        // Tự động tạo tài khoản nếu chưa có
        var user = await _googleAuthService.AutoProvisionUserAsync(info);
        if (user == null)
        {
            _logger.LogWarning("Could not create user from external provider.");
            return RedirectToAction(nameof(Login));
        }

        await _googleAuthService.AddLoginAsync(user, info);
        await _googleAuthService.SignInAsync(user);
        _logger.LogInformation("User auto-provisioned and signed in with Google.");
        return RedirectToLocal(returnUrl);
    }


    public async Task<IActionResult> Logout(string returnUrl = null)
    {
        await _loginService.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }
    // GET: /Account/Error
    [HttpGet]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
}
