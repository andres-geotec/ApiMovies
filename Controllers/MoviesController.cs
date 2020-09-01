using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ApiMovies.Models;
using ApiMovies.Models.Dtos;
using ApiMovies.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovies.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiMovies")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class MoviesController: ControllerBase
    {
        private readonly IMovieRepository _movieRepo;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public MoviesController(IMovieRepository movieRepo, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _movieRepo = movieRepo;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// Obtener todas las palículas.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<MovieDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetMovies()
        {
            var movies = _movieRepo.GetMovies();
            var moviesDto = new List<MovieDto>();
            foreach (var item in movies)
            {
                moviesDto.Add(_mapper.Map<MovieDto>(item));
            }
            return Ok(moviesDto);
        }

        /// <summary>
        /// Obtener una película por Id.
        /// </summary>
        /// <param name="id">El Id de la palícula.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id:int}", Name = "GetMovie")]
        [ProducesResponseType(200, Type = typeof(MovieDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetMovie(int id)
        {
            var movie = _movieRepo.GetMovie(id);
            if (movie == null) {
                return NotFound();
            }
            var movieDto = _mapper.Map<MovieDto>(movie);
            return Ok(movieDto);
        }

        /// <summary>
        /// Obtener película por Id de la categoría.
        /// </summary>
        /// <param name="categoryId">El Id de la categoría.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("ByCategory/{categoryId:int}")]
        [ProducesResponseType(200, Type = typeof(List<MovieDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetMoviesByCategory(int categoryId)
        {
            var movies = _movieRepo.GetMoviesByCategory(categoryId);
            if (movies == null)
            {
                return NotFound();
            }

            var moviesDto = new List<MovieDto>();
            foreach (var item in movies)
            {
                moviesDto.Add(_mapper.Map<MovieDto>(item));
            }
            return Ok(moviesDto);
        }

        /// <summary>
        /// Buscar película por nombre.
        /// </summary>
        /// <param name="name">El Nombre de la película.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Search")]
        public IActionResult SearchMovie(string name)
        {
            try
            {
                var result = _movieRepo.SearchMovie(name);
                if (result.Any())
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando los datos");
            }
        }

        /// <summary>
        /// Crea una nueva película.
        /// </summary>
        /// <param name="movieCreateDto">La película que se va a crear.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(MovieCreateDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateMovie([FromForm]MovieCreateDto movieCreateDto)
        {
            if (movieCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_movieRepo.MovieExists(movieCreateDto.Name))
            {
                ModelState.AddModelError("", "La pelicula ya existe.");
                return StatusCode(404, ModelState);
            }

            // Subida de archivos
            var file = movieCreateDto.Image;
            var mainRoute = _hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            if (file.Length > 0) // existe la imagen?
            {
                // Nueva imagen
                var photoName = Guid.NewGuid().ToString();
                var folder = Path.Combine(mainRoute, @"photos");
                var extension = Path.GetExtension(files[0].FileName);

                using(var fileStream = new FileStream(Path.Combine(folder, photoName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                movieCreateDto.ImageRout = @"\photos\" + photoName + extension;
            }

            var movie = _mapper.Map<Movie>(movieCreateDto);
            if (!_movieRepo.CreateMovie(movie))
            {
                ModelState.AddModelError("", $"Algo salió mal gurdando la pelicula {movie.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetMovie", new {id = movie.Id}, movie);
        }

        /// <summary>
        /// Actualizar una película existente.
        /// </summary>
        /// <param name="id">El Id de la película.</param>
        /// <param name="movieUpdateDto">La película con datos modificados.</param>
        [HttpPatch("{id:int}", Name = "UpdateMovie")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateMovie(int id, [FromBody]MovieUpdateDto movieUpdateDto)
        {
            if (movieUpdateDto == null || id != movieUpdateDto.Id)
            {
                return BadRequest(ModelState);
            }

            var movie = _mapper.Map<Movie>(movieUpdateDto);
            if (!_movieRepo.UpdateMovie(movie))
            {
                ModelState.AddModelError("", $"Algo salió mal al actualizar la pelicula {movie.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        /// <summary>
        /// Borrar una película existente.
        /// </summary>
        /// <param name="id">El Id de la película.</param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "DeleteMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteMovie(int id)
        {
            if (!_movieRepo.MovieExists(id))
            {
                return NotFound();
            }

            var movie = _movieRepo.GetMovie(id);
            if (!_movieRepo.DeleteMovie(movie))
            {
                ModelState.AddModelError("", $"Algo salió mal borrando la pelicula {movie.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}