using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
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
            if (_userRepository.IsLoginTaken(login))
            {
                return Json(false);
            }

            return Json(true);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult ValidateIfLoginExists(string login)
        {
            return Json(_userRepository.IsLoginTaken(login));
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult ValidateEmail(string email)
        {
            if (_userRepository.IsEmailTaken(email))
            {
                return Json(false);
            }

            return Json(true);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult ValidatePassword(string login, string password)
        {
            return Json(_userRepository.IsPasswordCorrect(login, password));
        }

        public async Task<IActionResult> ValidateEmailExceptUser(string email, ObjectId userId)
        {
            return Json(await _userRepository.IsEmailTakenExceptUser(email, userId));
        }

        public bool ValidateDateOfBirth(DateTime dateOfBirth)
        {
            return dateOfBirth < DateTime.Now.AddYears(-16) && dateOfBirth > DateTime.Now.AddYears(-80);
        }

        public bool ValidateExamDate(DateTime examDate)
        {
            return examDate >= DateTime.Now;
        }
    }
}