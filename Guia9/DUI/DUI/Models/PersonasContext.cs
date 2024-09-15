using Microsoft.EntityFrameworkCore;

namespace DUI.Models
{
    public class PersonasContext : DbContext
        {
            public PersonasContext(DbContextOptions<PersonasContext> options) : base(options) { }

            public DbSet<Persona> Personas { get; set; }
        }
    }
