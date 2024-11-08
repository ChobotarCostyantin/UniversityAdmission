using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UniversityAdmission.Data.Repos;

namespace UniversityAdmission.Controllers
{
    public class ValidationController : Controller
    {
        private readonly UserRepository _userRepository;

        public ValidationController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ValidateLogin(string login)
        {
            return Json(!_userRepository.IsLoginTaken(login));
        }

        public IActionResult ValidateEmail(string email)
        {
            return Json(!_userRepository.IsEmailTaken(email));
        }

        public IActionResult ValidatePassword(string email, string password)
        {
            return Json(_userRepository.IsPasswordCorrect(email, password));
        }
    }
}