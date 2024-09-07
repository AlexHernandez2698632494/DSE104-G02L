using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCPelicula.Models
{
    public class Pelicula
    {
        public int ID { get; set; }
        [StringLength(250)]
        [Required]
        public string Titulo { get; set; }
        public DateTime FechaLanzamiento { get; set; }

        //Propiedad para la llave foránea
        [Required]
        [ForeignKey("Genero")]
        public int? Generold { get; set; }
        //Propiedad de navegación
        public Genero Genero { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [StringLength(250)]
        [Required]
        public string Director { get; set; }
    }
}