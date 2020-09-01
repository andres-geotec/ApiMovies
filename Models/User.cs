using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Models
{
#pragma warning disable CS1591 // Falta el comentario XML para el tpo o miembro visible públicamente
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserA { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
#pragma warning disable CS1591 // Falta el comentario XML para el tpo o miembro visible públicamente
}