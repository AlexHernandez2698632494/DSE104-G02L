using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Entites.DTO
{
    public class AutorDto
    {
        public int Codigo { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50, ErrorMessage = "EL nombre no puede tener mas de 50 caracteres")]
        public string NombreAutor { get; set; }= string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(50, ErrorMessage = "EL apellido no puede tener mas de 50 caracteres")]
        public string ApellidoAutor { get; set; } = string.Empty;
    }
}
