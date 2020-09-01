using System.Collections.Generic;
using ApiMovies.Models;
using ApiMovies.Repository.IRepository;
using ApiMovies.DataAccess;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ApiMovies.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly PostgreSqlContext _db;

        public MovieRepository(PostgreSqlContext db)
        {
            _db = db;
        }

        public bool MovieExists(int id)
        {
            bool valor = _db.movies.Any(movie => movie.Id == id);
            return valor;
        }

        public bool MovieExists(string name)
        {
            bool valor = _db.movies.Any(movie => movie.Name.ToLower().Trim() == name.ToLower().Trim());
            return valor;
        }

        public bool CreateMovie(Movie movie)
        {
            _db.movies.Add(movie);
            return Save();
        }

        public bool DeleteMovie(Movie movie)
        {
            _db.movies.Remove(movie);
            return Save();
        }

        public ICollection<Movie> GetMovies()
        {
            return _db.movies.OrderBy(movie => movie.Name).ToList();
        }

        public Movie GetMovie(int id)
        {
            return _db.movies.FirstOrDefault(movie => movie.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0? true: false;
        }

        public bool UpdateMovie(Movie movie)
        {
            _db.movies.Update(movie);
            return Save();
        }

        public ICollection<Movie> GetMoviesByCategory(int categoryId)
        {
            return _db.movies.Include(model => model.Category).Where(category => category.categoryId == categoryId).ToList();
        }

        public IEnumerable<Movie> SearchMovie(string name)
        {
            IQueryable<Movie> query = _db.movies;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(movie => movie.Name.ToLower().Contains(name.ToLower()) || movie.Description.ToLower().Contains(name.ToLower()));
            }
            return query.ToList();
        }
    }
}