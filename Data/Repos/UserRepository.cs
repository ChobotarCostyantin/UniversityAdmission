using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using UniversityAdmission.Models.Entities;
using UniversityAdmission.Services;
using UniversityAdmission.Settings;

namespace UniversityAdmission.Data.Repos
{
    public class UserRepository
    {
        private readonly UniversityAdmissionDBContext _context;
        public UserRepository(UniversityAdmissionDBContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task AddUser(User user)
        {
            _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Permission>> GetUserPermissionsAsync(ObjectId userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            return RolePermissions.Permissions[user.Role];
        }

        public User GetUserByToken(string token)
        {
            var userId = JwtService.GetUserIdFromToken(token);
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            return user;
        }

        public bool IsUser(ObjectId userId)
        {
            return _context.Users.Any(x => x.Id == userId);
        }

        public bool IsLoginTaken(string login)
        {
            return _context.Users.Any(u => u.Login == login);
        }

        public bool IsEmailTaken(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool IsPasswordCorrect(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            return user != null;
        }
    }
}