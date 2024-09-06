using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Libro
    {
        public int LibroID { get; set; }

        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; }

        [Required]
        public DateTime FechaPublicacion { get; set; }

        // Clave foránea para Autor
        public int AutorID { get; set; }
        public Autor Autor { get; set; }

        // Clave foránea para Categoria
        public int CategoriaID { get; set; }
        public Categoria Categoria { get; set; }
    }
}
