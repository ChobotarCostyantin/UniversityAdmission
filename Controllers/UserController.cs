using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using UniversityAdmission.Data.Repos;
using UniversityAdmission.Models.DTO;

namespace UniversityAdmission.Controllers
{
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;
        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAll();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(ObjectId id)
        {
            if (id == ObjectId.Empty)
            {
                return NotFound();
            }

            var selectedUser = await _userRepository.GetById(id);
            if (selectedUser == null)
            {
                return NotFound();
            }

            var user = new EditRequest
            {
                Id = selectedUser.Id,
                Login = selectedUser.Login,
                Email = selectedUser.Email,
                Role = selectedUser.Role,
                Password = selectedUser.Password
            };
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditRequest request)
        {
            await _userRepository.UpdateUser(request);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ObjectId id)
        {
            await _userRepository.DeleteUser(id);
            return RedirectToAction("Index");
        }
    }
}