using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using static ApiMovies.Models.Movie;

namespace ApiMovies.Models
{
    public class MovieUpdateDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //public string ImageRout { get; set; }
        
        public string Description { get; set; }

        public string Duration { get; set; }

        public ClassificationType Classification { get; set; }

        public int categoryId { get; set; }
    }
}