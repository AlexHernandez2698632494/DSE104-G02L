using Microsoft.EntityFrameworkCore;

namespace ProductoAPI.Models
{
    public class ProductoContext : DbContext
    {
        public ProductoContext(DbContextOptions<ProductoContext> options) :base(options) { }
        public DbSet<Productos> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Productos>().HasData(new Productos { Id = 1, Nombre = "Laptop", Categoria = "Electtronica", Descripcion = "una laptop de alto rendimiento." } ,
            new Productos
            {
                Id = 2,
                Nombre = "Smartphone",
                Categoria = "Electronica",
                Descripcion = "Un smartphone de nueva generacion"
            }, new Productos
            {
                Id=3,Nombre="silla de escritorio",Categoria="mueble",Descripcion="una silla de escritorio comoda"
            });
        }
    }
}
