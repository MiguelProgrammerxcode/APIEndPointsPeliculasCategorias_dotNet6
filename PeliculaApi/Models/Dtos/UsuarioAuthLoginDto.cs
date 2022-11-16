using System.ComponentModel.DataAnnotations;

namespace PeliculaApi.Models.Dtos;

public class UsuarioAuthLoginDto
{
    [Required(ErrorMessage = "El usuario es obligatorio")]
    public string Usuario { get; set; }
    [Required(ErrorMessage = "La contrasenia es obligatoria")]
    public string Password { get; set; }
}