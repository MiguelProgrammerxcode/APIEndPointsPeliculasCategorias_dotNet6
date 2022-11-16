using PeliculaApi.Models;

namespace PeliculaApi.Repository.IRepository;

public interface IPeliculaRepository
{
    ICollection<Pelicula> GetPeliculas();
    ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId);
    Pelicula GetPelicula(int id);
    bool ExistePelicula(string nombre);
    IEnumerable<Pelicula> BuscarPelicula(string nombre);
    bool ExistePelicula(int id);
    bool CrearPelicula(Pelicula pelicula);
    bool ActualizarPelicula(Pelicula pelicula);
    bool BorrarPelicula(Pelicula pelicula);
    bool Guardar();
}