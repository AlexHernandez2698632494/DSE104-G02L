using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Categoria
    {
        public int CategoriaID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        // Relación: Una categoría puede contener varios libros
        public ICollection<Libro> Libros { get; set; }
    }
}
