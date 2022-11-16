using System.ComponentModel.DataAnnotations;

namespace PeliculaApi.Models.Dtos;

public class CategoriaDto
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; }
    public DateTime FechaCreacion { get; set; }
}