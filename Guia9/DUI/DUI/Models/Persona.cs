using System.ComponentModel.DataAnnotations;

namespace DUI.Models
{
    public class Persona
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El primer nombre es requerido")]
        [MaxLength(100, ErrorMessage = "El primer nombre no puede exceder los 100 caracteres")]
        public string PrimerNombre { get; set; }

        [MaxLength(100, ErrorMessage = "El segundo nombre no puede exceder los 100 caracteres")]
        public string SegundoNombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es requerido")]
        [MaxLength(100, ErrorMessage = "El primer apellido no puede exceder los 100 caracteres")]
        public string PrimerApellido { get; set; }

        [MaxLength(100, ErrorMessage = "El segundo apellido no puede exceder los 100 caracteres")]
        public string SegundoApellido { get; set; }

        [Required(ErrorMessage = "El DUI es requerido")]
        [RegularExpression(@"^\d{8}-\d{1}$", ErrorMessage = "El formato del DUI es inválido")]
        public string DUI { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        [DataType(DataType.Date, ErrorMessage = "La fecha de nacimiento debe ser válida")]
        public DateTime FechaNacimiento { get; set; }
    }
}
