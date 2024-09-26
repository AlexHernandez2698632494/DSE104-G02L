using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EventManagementAPI.Models
{
    public class Participante
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int EventoId { get; set; } // Este campo es obligatorio y solo se necesita el ID

        // Hacemos que la relación con Evento no sea obligatoria para la validación
        [JsonIgnore]
        public Evento? Evento { get; set; }
    }
}
