using PeliculaApi.Models;

namespace PeliculaApi.Repository.IRepository;

public interface ICategoriaRepository
{
    ICollection<Categoria> GetCategorias();
    Categoria GetCategoria(int id);
    bool ExisteCategoria(string nombre);
    bool ExisteCategoria(int id);
    bool CrearCategoria(Categoria categoria);
    bool ActualizarCategoria(Categoria categoria);
    bool BorrarCategoria(Categoria categoria);
    bool Guardar();
}