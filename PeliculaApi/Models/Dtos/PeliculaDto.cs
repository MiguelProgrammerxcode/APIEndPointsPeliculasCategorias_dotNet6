using System.ComponentModel.DataAnnotations;
using static PeliculaApi.Models.Pelicula;

namespace PeliculaApi.Models.Dtos;

public class PeliculaDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; }
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string RutaImagen { get; set; }
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Descripcion { get; set; }
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Duracion { get; set; }

    public TipoClasificacion Clasificacion { get; set; }

    // Relacion foreing key
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; }
}