using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Autor
    {
        public int AutorID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(100)]
        public string Apellido { get; set; }

        // Relación: Un autor puede tener varios libros
        public ICollection<Libro> Libros { get; set; }
    }
}
