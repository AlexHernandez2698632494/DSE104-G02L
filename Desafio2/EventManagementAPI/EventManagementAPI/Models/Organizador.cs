using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EventManagementAPI.Models
{
    public class Organizador
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Cargo { get; set; }

        [Required]
        public int EventoId { get; set; }

        // Esta propiedad no se incluirá en la respuesta JSON
        [JsonIgnore]
        public Evento? Evento { get; set; }
    }
}
