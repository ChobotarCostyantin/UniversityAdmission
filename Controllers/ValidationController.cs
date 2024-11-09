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

        [AcceptVerbs("Get", "Post")]
        public IActionResult ValidateLogin(string login)
        {
            return Json(!_userRepository.IsLoginTaken(login));
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult ValidateEmail(string email)
        {
            return Json(!_userRepository.IsEmailTaken(email));
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult ValidatePassword(string login, string password)
        {
            return Json(_userRepository.IsPasswordCorrect(login, password));
        }
    }
}