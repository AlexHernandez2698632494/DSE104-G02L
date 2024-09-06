using System.ComponentModel.DataAnnotations;

namespace ProductoAPI.Models
{
    public class Productos
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? Categoria { get; set; }

    }
}
