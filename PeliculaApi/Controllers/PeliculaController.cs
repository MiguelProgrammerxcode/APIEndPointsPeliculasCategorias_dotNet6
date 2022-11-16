using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeliculaApi.Models;
using PeliculaApi.Models.Dtos;
using PeliculaApi.Repository.IRepository;

namespace PeliculaApi.Controllers
{
    [Authorize]
    [Route("api/Pelicula")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiPeliculas")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class PeliculaController : ControllerBase
    {
        private readonly IPeliculaRepository _peliculaRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostingEnviroment;

        public PeliculaController(IPeliculaRepository peliculaRepository, IMapper mapper, IWebHostEnvironment hostingEnviroment)
        {
            _peliculaRepository = peliculaRepository;
            _mapper = mapper;
            _hostingEnviroment = hostingEnviroment;
        }
        
        [AllowAnonymous]
        [HttpGet(Name = "GetAllPeliculas")]
        public IActionResult GetPeliculas()
        {
            var listaPeliculas = _peliculaRepository.GetPeliculas();
            var listaPeliculasDto = listaPeliculas.Select(fila => _mapper.Map<PeliculaDto>(fila)).ToList();
            return Ok(listaPeliculasDto);
        }
        
        [AllowAnonymous]
        [HttpGet("{peliculaId:int}", Name = "GetPeliculaById")]
        public IActionResult GetPelicula(int peliculaId)
        {
            var itemPelicula = _peliculaRepository?.GetPelicula(peliculaId);

            if (itemPelicula is null)
                return NotFound();

            var itemPeliculaDto = _mapper.Map<PeliculaDto>(itemPelicula);
            return Ok(itemPeliculaDto);
        }
        
        [AllowAnonymous]
        [HttpGet("{categoriaId:int}", Name = "GetPeliculasEnCategoria/")]
        public IActionResult GetPeliculasEnCategoria(int categoriaId)
        {
            var listaPelicula = _peliculaRepository?.GetPeliculasEnCategoria(categoriaId);

            if (listaPelicula is null)
                return NotFound();
            
            var listaPeliculaDto = listaPelicula.Select(fila => _mapper.Map<PeliculaDto>(fila)).ToList();
            return Ok(listaPeliculaDto);
        }
        
        [HttpGet("GetPeliculaByName")]
        public IActionResult GetPeliculaByName(string name)
        {
            try
            {
                var resultado = _peliculaRepository.BuscarPelicula(name);
                if (resultado.Any())
                    return Ok(resultado);

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error: recuperando datos de la aplicacion");
            }
        }
        
        [HttpPost(Name = "CreatePelicula")]
        public IActionResult CreatePelicula([FromForm] PeliculaCreateDto peliculaCreateDto)
        {
            // Validaciones
            if (peliculaCreateDto is null)
                return BadRequest(ModelState);

            if (_peliculaRepository.ExistePelicula(peliculaCreateDto.Nombre!))
            {
                ModelState.AddModelError("Mensaje", "La pelicula ya existe");
                return StatusCode(404, ModelState);
            }
            
            // Subida de archivos
            var file = peliculaCreateDto.Foto;
            var rutaPrincipal = _hostingEnviroment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (file!.Length > 0)
            {
                //Nueva imagen
                var nombreFoto = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"fotos");
                var extension = Path.GetExtension(files[0].FileName);

                using var fileStreams = new FileStream(Path.Combine(subidas, nombreFoto + extension), FileMode.Create);
                files[0].CopyTo(fileStreams);
            }
            
            var pelicula = _mapper.Map<Pelicula>(peliculaCreateDto);

            if (_peliculaRepository.CrearPelicula(pelicula)) 
                return CreatedAtRoute("GetPeliculaById", new { id = pelicula.Id}, pelicula);
            
            ModelState.AddModelError("Mensaje",
                $"Ocurrió un incidente en la creación del registro: { pelicula.Nombre }");
            return StatusCode(500, ModelState);
        }

        [HttpPatch("{peliculaId:int}", Name = "ActualizarPelicula")]
        public IActionResult ActualizarPelicula(int peliculaId, [FromBody] PeliculaUpdateDto peliculaUpdateDto)
        {
            if (peliculaUpdateDto is null || peliculaId != peliculaUpdateDto.Id)
                return BadRequest(ModelState);

            var pelicula = _mapper.Map<Pelicula>(peliculaUpdateDto);

            if (_peliculaRepository.ActualizarPelicula(pelicula))
                return NoContent();
            
            ModelState.AddModelError("Mensaje",
                $"Ocurrió un incidente en la actualización del registro: { pelicula.Nombre }");
            return StatusCode(500, ModelState);
        }
        
        [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        public IActionResult BorrarPelicula(int peliculaId)
        {
            if (!_peliculaRepository.ExistePelicula(peliculaId))
                return NotFound();

            var pelicula = _peliculaRepository.GetPelicula(peliculaId);
            
            if (_peliculaRepository.BorrarPelicula(pelicula))
                return NoContent();
            
            ModelState.AddModelError("Mensaje",
                $"Ocurrió un incidente en la eliminación del registro: { pelicula.Nombre }"); 
            return StatusCode(500, ModelState);
        }
    }
}
