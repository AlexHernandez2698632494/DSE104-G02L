using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace VehiculosAPI.Models
{
    public class VehiculoDBContext : IdentityDbContext<Usuario>
    {
        public VehiculoDBContext(DbContextOptions<VehiculoDBContext> options) : base(options) { }

        public DbSet<Marcas> Marcas { get; set; }
        public DbSet<Modelo> Modelos { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
    }
}
