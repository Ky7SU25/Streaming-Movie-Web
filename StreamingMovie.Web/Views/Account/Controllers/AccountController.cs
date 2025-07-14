using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Application.Interfaces;
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

    public AccountController(ILoginService loginService, ILogger<AccountController> logger, IHttpContextAccessor httpContextAccessor)
    {
        _loginService = loginService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    // GET: /Account/Login
    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
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
    public IActionResult Register()
    {
        return View();
    }

    // POST: /Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            //var user = new User { UserName = model.Email, Email = model.Email };
            //var result = await _userManager.CreateAsync(user, model.Password);
            //if (result.Succeeded)
            //{
            //    await _signInManager.SignInAsync(user, isPersistent: false);
            //    return RedirectToAction(nameof(HomeController.Index), "Home");
            //}
            //foreach (var error in result.Errors)
            //{
            //    ModelState.AddModelError(string.Empty, error.Description);
            //}
        }
        return View(model);
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
