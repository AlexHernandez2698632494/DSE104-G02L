using System.ComponentModel.DataAnnotations;

namespace VehiculosAPI.Models

{
    public class Marcas
    {
        [Key]
       public int ID { get; set; }
        public string Nombre { get; set; }
        public List<Modelo> Modelos { get; set; } = new List<Modelo>();
    }
}
