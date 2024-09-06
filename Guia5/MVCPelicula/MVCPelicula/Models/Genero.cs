using System.ComponentModel.DataAnnotations;

namespace MVCPelicula.Models
{
    public class Genero
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        //Propiedad de navegación
        public virtual ICollection<Pelicula> Peliculas { get; set; }
    }
}
