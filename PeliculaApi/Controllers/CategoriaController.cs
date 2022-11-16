using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeliculaApi.Models;
using PeliculaApi.Models.Dtos;
using PeliculaApi.Repository.IRepository;

namespace PeliculaApi.Controllers
{
    [Authorize]
    [Route("api/Categorias")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiCategorias")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;
        
        public CategoriaController(ICategoriaRepository categoriaRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Obtener todas las categorias de las peliculas, tabla: "Categorias".
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet(Name = "GetAllCategorias")]
        [ProducesResponseType(200, Type = typeof(List<CategoriaDto>))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetCategorias()
        {
            var listaCategorias = _categoriaRepository.GetCategorias();
            var listaCategoriasDto = listaCategorias.Select(fila => _mapper.Map<CategoriaDto>(fila)).ToList();
            return Ok(listaCategoriasDto);
        }
        /// <summary>
        /// Obtener una categoria por ID.
        /// </summary>
        /// <param name="id">Este parámetro es el ID de la categoría.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id:int}", Name = "GetCategoriaById")]
        public IActionResult GetCategoria(int id)
        {
            var itemCategoria = _categoriaRepository?.GetCategoria(id);

            if (itemCategoria is null)
                return NotFound();

            var itemCategoriaDto = _mapper.Map<CategoriaDto>(itemCategoria);
            return Ok(itemCategoriaDto);
        }
        
        /// <summary>
        /// Crear una nueva categoria.
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "CreateCategoria")]
        [ProducesResponseType(201, Type = typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategoria([FromBody] CategoriaDto categoriaDto)
        {
            // Validaciones
            if (categoriaDto is null)
                return BadRequest(ModelState);

            if (_categoriaRepository.ExisteCategoria(categoriaDto.Nombre!))
            {
                ModelState.AddModelError("Mensaje", "La categoria ya existe");
                return StatusCode(404, ModelState);
            }

            var categoria = _mapper.Map<Categoria>(categoriaDto);

            if (_categoriaRepository.CrearCategoria(categoria)) 
                return CreatedAtRoute("GetCategoriaById", new { id = categoria.Id}, categoria);
            
            ModelState.AddModelError("Mensaje",
                $"Ocurrió un incidente en la creación del registro: {categoria.Nombre}");
            return StatusCode(500, ModelState);
        }
        
        /// <summary>
        /// Actualizar una categoria existente.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoriaDto"></param>
        /// <returns></returns>
        [HttpPatch("{id:int}", Name = "ActualizarCategoria")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarCategoria(int id, [FromBody] CategoriaDto categoriaDto)
        {
            if (categoriaDto is null || id != categoriaDto.Id)
                return BadRequest(ModelState);

            var categoria = _mapper.Map<Categoria>(categoriaDto);

            if (_categoriaRepository.ActualizarCategoria(categoria))
                return NoContent();
            
            ModelState.AddModelError("Mensaje",
                $"Ocurrió un incidente en la actualización del registro: {categoria.Nombre}");
            return StatusCode(500, ModelState);
        }
        
        /// <summary>
        /// Borrar una categoria existente por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "BorrarCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarCategoria(int id)
        {
            if (!_categoriaRepository.ExisteCategoria(id))
                return NotFound();

            var categoria = _categoriaRepository.GetCategoria(id);
            
            if (_categoriaRepository.BorrarCategoria(categoria))
                return NoContent();
            
            ModelState.AddModelError("Mensaje",
                $"Ocurrió un incidente en la eliminación del registro: {categoria.Nombre}");
            return StatusCode(500, ModelState);
        }
    }
}
