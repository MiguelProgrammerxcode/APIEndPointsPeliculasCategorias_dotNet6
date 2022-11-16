using PeliculaApi.Data;
using PeliculaApi.Models;
using PeliculaApi.Repository.IRepository;
using PeliculaApi.Utilitarios;

namespace PeliculaApi.Repository;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UsuarioRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ICollection<Usuario> GetUsuarios()
    {
        return _dbContext.Usuario.OrderBy(u => u.UsuarioA).ToList();
    }

    public Usuario GetUsuario(int usuarioId)
    {
        return _dbContext.Usuario.FirstOrDefault(u => u.Id == usuarioId)!;
    }

    public bool ExisteUsuario(string usuario)
    {
        return _dbContext.Usuario.Any(u => u.UsuarioA!.Trim().ToLower().Equals(usuario.Trim().ToLower()));
    }

    public Usuario Registro(Usuario usuario, string password)
    {
        Util.CrearPasswordHash(password, out var passwordHash, out var passwordSalt);

        usuario.PasswordHash = passwordHash;
        usuario.PasswordSalt = passwordSalt;

        _dbContext.Usuario.Add(usuario);
        Guardar();
        return usuario;
    }

    public Usuario Login(string usuario, string password)
    {
        var user = _dbContext.Usuario.FirstOrDefault(x => x.UsuarioA!.Equals(usuario));

        if (user is null) return null!;

        return !Util.VerificaPasswordHash(password, user.PasswordHash!, user.PasswordSalt!) ? null! : user;
    }

    public bool Guardar()
    {
        return _dbContext.SaveChanges() >= 0;
    }
}