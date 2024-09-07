using Microsoft.EntityFrameworkCore;

namespace Persona.Models
{
    public class PersonasDBContext : DbContext
        {
            public PersonasDBContext(DbContextOptions options) : base(options)
            {
            }
            public DbSet<Persona> Peliculas { get; set; }
        }
    }
}
}
