using PeliculaApi.Models;

namespace PeliculaApi.Repository.IRepository;

public interface IUsuarioRepository
{
    ICollection<Usuario> GetUsuarios();
    Usuario GetUsuario(int usuarioId);
    bool ExisteUsuario(string usuario);
    Usuario Registro(Usuario usuario, string password);
    Usuario Login(string usuario, string password);
    bool Guardar();
}