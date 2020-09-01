using System.Collections.Generic;

using ApiMovies.Models;

namespace ApiMovies.Repository.IRepository
{
    public interface IMovieRepository
    {
        ICollection<Movie> GetMovies();

        ICollection<Movie> GetMoviesByCategory(int categoryId);

        Movie GetMovie(int id);

        bool MovieExists(int id);

        bool MovieExists(string name);

        IEnumerable<Movie> SearchMovie(string name);

        bool CreateMovie(Movie movie);

        bool UpdateMovie(Movie movie);
        
        bool DeleteMovie(Movie movie);
        
        bool Save();
    }
}