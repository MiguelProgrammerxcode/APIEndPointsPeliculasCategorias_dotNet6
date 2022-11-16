

using System.ComponentModel.DataAnnotations;

namespace PeliculaApi.Models.Dtos;

public class UsuarioAuthDto
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "El usuario es obligatorio")]
    public string Usuario { get; set; }
    [Required(ErrorMessage = "La contrasenia es obligatoria")]
    [StringLength(10, MinimumLength = 4, ErrorMessage = "La contrasenia debe estar entre 4 y 10 caracteres")]
    public string Password { get; set; }
    
}