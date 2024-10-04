namespace VehiculosAPI.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }
        public int Año { get; set; }

        public int ModeloId { get; set; }
        public Modelo Modelo { get; set; }
    }

}
