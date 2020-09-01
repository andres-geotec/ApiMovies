using System.Collections.Generic;
using ApiMovies.Models;
using ApiMovies.Repository.IRepository;
using ApiMovies.DataAccess;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;

namespace ApiMovies.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly PostgreSqlContext _db;

        public CategoryRepository(PostgreSqlContext db)
        {
            _db = db;
        }

        public bool CategoryExists(int id)
        {
            bool valor = _db.categorys.Any(c => c.Id == id);
            return valor;
        }

        public bool CategoryExists(string name)
        {
            bool valor = _db.categorys.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
            return valor;
        }

        public bool CreateCategory(Category category)
        {
            _db.categorys.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _db.categorys.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategorys()
        {
            return _db.categorys.OrderBy(c => c.Name).ToList();
        }

        public Category GetCategory(int id)
        {
            return _db.categorys.FirstOrDefault(c => c.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0? true: false;
        }

        public bool UpdateCategory(Category category)
        {
            _db.categorys.Update(category);
            return Save();
        }
    }
}