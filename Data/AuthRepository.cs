using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rpg_Class_Project.Models;

namespace rpg_Class_Project.Data
{
    public class AuthRepository : IAuthRepository
    {

        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var response = new ServiceResponse<int>();

            if(user != null && password != string.Empty && await UserExists(user.Username) == false)
            {
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                response.Data = user.Id;
                response.Success = true;
                response.Message = "Succesfully registered a new User";
            }
            else if (user != null && await UserExists(user.Username))
            {
                response.Data = -1;
                response.Success = false;
                response.Message = $"User with the username '{user.Username}' already exists, the User could not be created.";
            }
            else
            {
                response.Data = -1;
                response.Success = false;
                response.Message = "Could not register new User: The User was null, or the password was not filled in.";
            }

            return response;
        }

        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();

            if(username != string.Empty && password != string.Empty)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));

                if(user == null)
                {
                    response.Data = string.Empty;
                    response.Success = false;
                    response.Message = $"User with the username of '{username}' could not be found.";
                }
                else if(!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                {
                    response.Data = string.Empty;
                    response.Success = false;
                    response.Message = $"Incorrect password.";
                }
                else
                {
                    response.Data = user.Id.ToString();
                    response.Success = true;
                    response.Message = $"User was found, logging in now.";
                }
            }
            else
            {
                    response.Data = string.Empty;
                    response.Success = false;
                    response.Message = $"Username and/or password was not filled in. Please enter both a username and a password.";
            }
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()))
            {
                return true;
            }
            else return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();

            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);

            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }
    }
}