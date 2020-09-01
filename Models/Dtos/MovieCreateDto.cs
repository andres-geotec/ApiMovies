using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using static ApiMovies.Models.Movie;

namespace ApiMovies.Models
{
    public class MovieCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; }

        public string ImageRout { get; set; }

        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Description { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria")]
        public string Duration { get; set; }

        public ClassificationType Classification { get; set; }

        public int categoryId { get; set; }
    }
}