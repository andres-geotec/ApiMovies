using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Models.Dtos
{
    public class UserDto
    {
        public string UserA { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}