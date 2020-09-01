using System.Collections.Generic;

using ApiMovies.Models;

namespace ApiMovies.Repository.IRepository
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategorys();

        Category GetCategory(int id);

        bool CategoryExists(int id);

        bool CategoryExists(string name);

        bool CreateCategory(Category category);

        bool UpdateCategory(Category category);

        bool DeleteCategory(Category category);
        
        bool Save();
    }
}