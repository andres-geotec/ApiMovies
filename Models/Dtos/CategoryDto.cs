using System.ComponentModel.DataAnnotations;
using System;

namespace ApiMovies.Models.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; }

        public DateTime Timestamp { get; set; }
    }
}