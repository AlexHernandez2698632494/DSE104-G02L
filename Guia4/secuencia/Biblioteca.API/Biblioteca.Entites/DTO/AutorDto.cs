using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Entities.DTO
{
    public class AutorDto
    {
        public int Codigo { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener mas de 50 caracteres.")]
        public string NombreAutor { get; set; } = string.Empty;
        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener mas de 50 caracteres.")]
        public string ApellidoAutor { get; set; } = string.Empty;
    }
}