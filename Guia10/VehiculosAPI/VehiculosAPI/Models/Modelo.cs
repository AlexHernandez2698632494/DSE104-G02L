namespace VehiculosAPI.Models
{
    public class Modelo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public int MarcaId { get; set; }
        public Marcas Marca { get; set; }

        public List<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
    }

}
