using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PeliculaApi.Models;
using PeliculaApi.Models.Dtos;
using PeliculaApi.Repository.IRepository;

namespace PeliculaApi.Controllers
{
    [Authorize]
    [Route("api/Usuario")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiUsuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UsuarioController(IUsuarioRepository usuarioRepository, IMapper mapper, IConfiguration config)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _config = config;
        }

        [HttpGet("GetAllUsuarios")]
        public IActionResult GetAllUsuarios()
        {
            var listaUsuarios = _usuarioRepository.GetUsuarios();
            var listaUsuariosDto = listaUsuarios.Select(fila => _mapper.Map<UsuarioDto>(fila)).ToList();
            return Ok(listaUsuariosDto);
        }
        
        [HttpGet("GetUsuario/{usuarioId:int}")]
        public IActionResult GetUsuario(int usuarioId)
        {
            var user = _usuarioRepository?.GetUsuario(usuarioId);

            if (user is null)
                return NotFound();

            var userDto = _mapper.Map<UsuarioDto>(user);
            return Ok(userDto);
        }

        [AllowAnonymous]
        [HttpPost("Registro")]
        public IActionResult Registro(UsuarioAuthDto usuarioAuthDto)
        {
            usuarioAuthDto.Usuario = usuarioAuthDto.Usuario!.ToLower();

            if (_usuarioRepository.ExisteUsuario(usuarioAuthDto.Usuario))
                return BadRequest("El usuario ya existe");

            var nuevoUser = new Usuario { UsuarioA = usuarioAuthDto.Usuario };
            var result = _usuarioRepository.Registro(nuevoUser, usuarioAuthDto.Password!);

            return Ok(result);
        }
        
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UsuarioAuthLoginDto usuarioAuthLoginDto)
        {
            var user = _usuarioRepository?.Login(usuarioAuthLoginDto.Usuario!, usuarioAuthLoginDto.Password!);

            if (user is null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UsuarioA!)
            };
        
            // Generacion de token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value!));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credenciales
            };
        
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}
