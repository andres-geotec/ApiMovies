using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Models.Dtos
{
    public class UserAuthDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string User { get; set; }

        [Required(ErrorMessage = "La contraseña de usuario es obligatoria")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "La contraseña debe estar entre 4 y 10 caracteres")]
        public string Password { get; set; }
    }
}