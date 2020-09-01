using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Models.Dtos
{
    public class UserAuthLoginDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string User { get; set; }

        [Required(ErrorMessage = "La contraseña de usuario es obligatoria")]
        public string Password { get; set; }
    }
}