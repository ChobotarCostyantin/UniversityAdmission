using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityAdmission.Data.Repos;
using UniversityAdmission.Models.DTO;
using UniversityAdmission.Models;
using UniversityAdmission.Services;

namespace UniversityAdmission.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserRepository _userRepository;
    private readonly UserService _userService;

    public HomeController(ILogger<HomeController> logger, UserRepository userRepository, UserService userService)
    {
        _logger = logger;
        _userRepository = userRepository;
        _userService = userService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult Privacy()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("access-cookie");
        return RedirectToAction("Index");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        await _userService.Register(request.Login, request.Email, request.Role, request.Password);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var token = await _userService.Login(request.Email, request.Password) ?? throw new Exception("Невірний логін або пароль");
        Response.Cookies.Append("access-cookie", token);
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
