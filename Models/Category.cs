using System.ComponentModel.DataAnnotations;
using System;

namespace ApiMovies.Models
{
#pragma warning disable CS1591 // Falta el comentario XML para el tpo o miembro visible públicamente
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
    }
#pragma warning disable CS1591 // Falta el comentario XML para el tpo o miembro visible públicamente
}