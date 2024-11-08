using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityAdmission.Data.Repos;
using UniversityAdmission.Models.Entities;
using UniversityAdmission.Settings;

namespace UniversityAdmission.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Register(string login, string email, Roles role, string password)
        {
            var user = new User
            {
                Login = login,
                Email = email,
                Role = role,
                Password = password
            };
            await _userRepository.AddUser(user);
            return user;
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _userRepository.GetUser(email, password);
            return JwtService.GenerateToken(user);
        }

        public bool IsSignedIn(string token)
        {
            var user = _userRepository.GetUserByToken(token);
            return user != null;
        }
    }
}