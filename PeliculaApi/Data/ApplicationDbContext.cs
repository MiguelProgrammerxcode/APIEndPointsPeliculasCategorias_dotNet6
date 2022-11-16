using Microsoft.EntityFrameworkCore;
using PeliculaApi.Models;

namespace PeliculaApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Categoria> Categoria { get; set; } = null!;
    public DbSet<Pelicula> Pelicula { get; set; } = null!;
    public DbSet<Usuario> Usuario { get; set; } = null!;
}