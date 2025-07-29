using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Web.Views.Shared.ViewModels;
using System.Diagnostics;

namespace StreamingMovie.Web.Views.Home.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewData["WelcomeMessage"] = $"Welcome {User.Identity.Name}!";
        }

        if (User.IsInRole("Admin"))
            return RedirectToAction("Index", "Admin");

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Error404()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

