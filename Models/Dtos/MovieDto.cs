using System.ComponentModel.DataAnnotations;
using static ApiMovies.Models.Movie;

namespace ApiMovies.Models.Dtos
{
    public class MovieDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La ruta de la imagen es obligatoria")]
        public string ImageRout { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Description { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria")]
        public string Duration { get; set; }

        public ClassificationType Classification { get; set; }

        public int categoryId { get; set; }

        public Category Category { get; set; }
    }
}