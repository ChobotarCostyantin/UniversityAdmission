using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityAdmission.Data.Repos;
using UniversityAdmission.Models.DTO;
using UniversityAdmission.Models;
using UniversityAdmission.Services;
using UniversityAdmission.Settings;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Controllers;

public class HomeController : Controller
{
    private readonly UserService _userService;
    private readonly UserRepository _userRepository;

    public HomeController(UserService userService, UserRepository userRepository)
    {
        _userService = userService;
        _userRepository = userRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("access-cookie");
        return RedirectToAction("Index");
    }

    [Authorize(Policy = "Administrator")]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        await _userService.Register(request.Login, request.Email, request.Role, request.Password);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var token = await _userService.Login(request.Login, request.Password);
        Response.Cookies.Append("access-cookie", token);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ForgotPassword(){
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO request){
        var user = await _userRepository.GetUserByLogin(request.Login);
        if(user != null) {
            request.Password = user.Password;
            return View(request);
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Користувач з таким логіном не знайдений.");
            return View(request);
        }
    }

    public IActionResult StatusCodeError(int statusCode)
    {
        return View(statusCode);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
