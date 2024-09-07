using Microsoft.EntityFrameworkCore;

namespace Empleados.Models
{
    public class EmpleadosDBContext:DbContext
    {
        public EmpleadosDBContext(DbContextOptions options) : base(options) 
        {
        }
        
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Proyectos> Proyectos { get; set; }
        public DbSet<Asignaciones> Asignaciones { get; set; }
    }
}
