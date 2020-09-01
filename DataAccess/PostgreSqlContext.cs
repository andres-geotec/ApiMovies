using Microsoft.EntityFrameworkCore;
using ApiMovies.Models;

namespace ApiMovies.DataAccess
{
    public class PostgreSqlContext: DbContext
    {
        public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options): base(options)
        {}

        public DbSet<Category> categorys { get; set; }

        public DbSet<Movie> movies { get; set; }

        public DbSet<User> users { get; set; }
    }
}