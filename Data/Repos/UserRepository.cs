using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using UniversityAdmission.Models.DTO;
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

        public async Task<User?> GetUser(string login, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
        }

        public async Task<User?> GetById(ObjectId id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(ObjectId userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateUser(EditRequest dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (user != null)
            {
                user.Login = dto.Login;
                user.Email = dto.Email;
                user.Role = dto.Role;
                user.Password = dto.Password;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Permission>> GetUserPermissionsAsync(ObjectId userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var permissions = RolePermissions.Permissions[user!.Role];
            Console.WriteLine($"User {userId} has permissions: {string.Join(", ", permissions)}");
            return permissions;
        }

        public User? GetUserByToken(string token)
        {
            var userId = JwtService.GetUserIdFromToken(token);
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            return user;
        }

        public async Task<bool> IsEmailTakenExceptUser(string email, ObjectId userId)
        {
            return await _context.Users.AnyAsync(x => x.Email == email && x.Id != userId);
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

        public bool IsPasswordCorrect(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            return user != null;
        }
    }
}