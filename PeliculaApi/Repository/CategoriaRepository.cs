using PeliculaApi.Data;
using PeliculaApi.Models;
using PeliculaApi.Repository.IRepository;

namespace PeliculaApi.Repository;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CategoriaRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ICollection<Categoria> GetCategorias()
    {
        return _dbContext.Categoria.OrderBy(c => c.Nombre).ToList();
    }

    public Categoria GetCategoria(int id)
    {
        return _dbContext.Categoria.FirstOrDefault(c => c.Id == id)!;
    }

    public bool ExisteCategoria(string nombre)
    {
        var valor = _dbContext.Categoria.Any(c => c.Nombre!.Trim().ToLower().Equals(nombre.Trim().ToLower()));
        return valor;
    }

    public bool ExisteCategoria(int id)
    {
        return _dbContext.Categoria.Any(c => c.Id == id);
    }

    public bool CrearCategoria(Categoria categoria)
    {
        _dbContext.Categoria.Add(categoria);
        return Guardar();
    }

    public bool ActualizarCategoria(Categoria categoria)
    {
        _dbContext.Categoria.Update(categoria);
        return Guardar();
    }

    public bool BorrarCategoria(Categoria categoria)
    {
        _dbContext.Categoria.Remove(categoria);
        return Guardar();
    }

    public bool Guardar()
    {
        return _dbContext.SaveChanges() >= 0;
    }
}