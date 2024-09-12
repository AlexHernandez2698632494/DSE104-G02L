using Microsoft.EntityFrameworkCore;

namespace LibrosApi.Models
{
    public class LibroDbContext : DbContext
    {
        public LibroDbContext(DbContextOptions<LibroDbContext> options) : base(options) { }
        public DbSet<Libro> Libros { get; set; }
    }
}
