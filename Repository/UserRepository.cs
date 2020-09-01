using System.Collections.Generic;
using ApiMovies.Models;
using ApiMovies.Repository.IRepository;
using ApiMovies.DataAccess;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;

namespace ApiMovies.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly PostgreSqlContext _db;

        public UserRepository(PostgreSqlContext db)
        {
            _db = db;
        }

        public User GetUser(int id)
        {
            return _db.users.FirstOrDefault(user => user.Id == id);
        }

        public ICollection<User> GetUsers()
        {
            return _db.users.OrderBy(user => user.UserA).ToList();
        }

        public User Login(string name, string password)
        {
            var user = _db.users.FirstOrDefault(user => user.UserA == name);
            if (user == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        public User Registry(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _db.users.Add(user);
            Save();
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {//clase 39
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var hashComputado = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < hashComputado.Length; i++)
                {
                    if (hashComputado[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {//clase 39
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0? true: false;
        }

        public bool UserExists(int id)
        {
            return _db.users.Any(user => user.Id == id)? true: false;
        }

        public bool UserExists(string name)
        {
            return _db.users.Any(user => user.UserA == name)? true: false;
        }
    }
}