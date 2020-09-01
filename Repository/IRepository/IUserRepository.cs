using System.Collections.Generic;

using ApiMovies.Models;

namespace ApiMovies.Repository.IRepository
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();

        User GetUser(int id);

        bool UserExists(int id);

        bool UserExists(string name);

        User Registry(User user, string password);

        User Login(string name, string password);
        
        bool Save();
    }
}