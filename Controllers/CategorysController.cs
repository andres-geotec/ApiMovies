using System;
using System.Collections.Generic;
using ApiMovies.Models;
using ApiMovies.Models.Dtos;
using ApiMovies.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovies.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiCategorys")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class CategorysController: ControllerBase
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;

        public CategorysController(ICategoryRepository categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener todas las categorías.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CategoryDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetCategorys()
        {
            var categorys = _categoryRepo.GetCategorys();
            var categorysDto = new List<CategoryDto>();
            foreach (var item in categorys)
            {
                categorysDto.Add(_mapper.Map<CategoryDto>(item));
            }
            return Ok(categorysDto);
        }

        /// <summary>
        /// Obtener una categoría por Id.
        /// </summary>
        /// <param name="id">El Id de la categoría.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id:int}", Name = "GetCategory")]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetCategory(int id)
        {
            var category = _categoryRepo.GetCategory(id);
            if (category == null) {
                return NotFound();
            }
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }

        /// <summary>
        /// Crea una nueva categoría.
        /// </summary>
        /// <param name="categoryDto">La categoría que se va a crear.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategory([FromBody]CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_categoryRepo.CategoryExists(categoryDto.Name))
            {
                ModelState.AddModelError("", "La categoria ya existe.");
                return StatusCode(404, ModelState);
            }

            var category = _mapper.Map<Category>(categoryDto);
            if (!_categoryRepo.CreateCategory(category))
            {
                ModelState.AddModelError("", $"Algo salió mal gurdando el registro {category.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCategory", new {id = category.Id}, category);
        }

        /// <summary>
        /// Actualizar una categoría existente.
        /// </summary>
        /// <param name="id">El Id de la categoría.</param>
        /// <param name="categoryDto">La categoría con datos modificados.</param>
        /// <returns></returns>
        [HttpPatch("{id:int}", Name = "UpdateCategory")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateCategory(int id, [FromBody]CategoryDto categoryDto)
        {
            if (categoryDto == null || id != categoryDto.Id)
            {
                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(categoryDto);
            if (!_categoryRepo.UpdateCategory(category))
            {
                ModelState.AddModelError("", $"Algo salió mal al actualizar el registro {category.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        /// <summary>
        /// Borrar una categoría existente.
        /// </summary>
        /// <param name="id">El Id de la categoría.</param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCategory(int id)
        {
            if (!_categoryRepo.CategoryExists(id))
            {
                return NotFound();
            }

            var category = _categoryRepo.GetCategory(id);
            if (!_categoryRepo.DeleteCategory(category))
            {
                ModelState.AddModelError("", $"Algo salió mal borrando el registro {category.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}