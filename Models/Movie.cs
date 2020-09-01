using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMovies.Models
{
#pragma warning disable CS1591 // Falta el comentario XML para el tpo o miembro visible públicamente
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageRout { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public enum ClassificationType { Siete, Trece, Dieciseis, Dieciocho }
        public ClassificationType Classification { get; set; }
        public DateTime Timestamp { get; set; }
        public int categoryId { get; set; }
        [ForeignKey("categoryId")]
        public Category Category { get; set; }
    }
#pragma warning disable CS1591 // Falta el comentario XML para el tpo o miembro visible públicamente
}