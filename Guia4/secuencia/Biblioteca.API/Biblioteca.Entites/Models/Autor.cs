using System.ComponentModel.DataAnnotations;
namespace Biblioteca.Entities.Models
{
public class Autor
    {
        [Key]
public int Id { get; set; }
public required string Nombre { get; set; }
public required string Apellido { get; set; }
    }
}