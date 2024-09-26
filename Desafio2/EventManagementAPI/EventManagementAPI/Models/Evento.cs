using System;
using System.ComponentModel.DataAnnotations;

namespace EventManagementAPI.Models
{
    public class Evento
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Nombre { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Lugar { get; set; }
    }
}
