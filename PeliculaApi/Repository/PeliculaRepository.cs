using Microsoft.EntityFrameworkCore;
using PeliculaApi.Data;
using PeliculaApi.Models;
using PeliculaApi.Repository.IRepository;

namespace PeliculaApi.Repository;

public class PeliculaRepository : IPeliculaRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PeliculaRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ICollection<Pelicula> GetPeliculas()
    {
        return _dbContext.Pelicula.ToList();
    }

    public ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId)
    {
        return _dbContext.Pelicula.Include(c => c.Categoria).Where(c => c.CategoriaId == categoriaId).ToList();
    }

    public Pelicula GetPelicula(int id)
    {
        return _dbContext.Pelicula.FirstOrDefault(p => p.Id == id)!;
    }

    public bool ExistePelicula(string nombre)
    {
        var exist = _dbContext.Pelicula.Any(p => p.Nombre!.Trim().ToLower().Equals(nombre.Trim().ToLower()));
        return exist;
    }

    public IEnumerable<Pelicula> BuscarPelicula(string nombre)
    {
        IQueryable<Pelicula> query = _dbContext.Pelicula;

        if (!string.IsNullOrEmpty(nombre))
            query = query.Where(e => e.Nombre!.Contains(nombre) || e.Descripcion!.Contains(nombre));

        return query.ToList();
    }

    public bool ExistePelicula(int id)
    {
        return _dbContext.Pelicula.Any(p => p.Id == id);
    }

    public bool CrearPelicula(Pelicula pelicula)
    {
        _dbContext.Pelicula.Add(pelicula);
        return Guardar();
    }

    public bool ActualizarPelicula(Pelicula pelicula)
    {
        _dbContext.Pelicula.Update(pelicula);
        return Guardar();
    }

    public bool BorrarPelicula(Pelicula pelicula)
    {
        _dbContext.Pelicula.Remove(pelicula);
        return Guardar();
    }

    public bool Guardar()
    {
        return _dbContext.SaveChanges() >= 0;
    }
}