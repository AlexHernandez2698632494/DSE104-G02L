using System.ComponentModel.DataAnnotations;

namespace LibroAPI.Models
{
    public class Libro
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(100)]
        public string Autor { get; set; }

        [Required]
        [Range(1600, 2024, ErrorMessage = "El año de publicación debe estar entre 1600 y 2024.")]
        public int AnioPublicacion { get; set; }
    }
}
