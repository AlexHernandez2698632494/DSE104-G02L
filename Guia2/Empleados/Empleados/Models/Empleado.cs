using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Empleados.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        [StringLength(250)]
        [Required]
        public string Name { get; set; }
        public string lastName { get; set; }
        public DateTime FechaContratacion {  get; set; }

        //Propiedades de las llaves Foraneas
        //llaves Foraneas Proyectos
        [Required]
        [ForeignKey("Proyectos")]
        public int? proyectosId { get; set; }
        //Propiedades de navegación
        public Proyectos proyectos { get; set; }

        //Propiedades de las llaves Foraneas
        //llaves Foraneas Asignaciones
        [Required]
        [ForeignKey("Asignaciones")]
        public int? asignacionId { get; set; }
        //Propiedades de navegación
        public Asignaciones asignaciones { get; set; }
        [Required]
        public string puesto { get; set; }
    }
}
