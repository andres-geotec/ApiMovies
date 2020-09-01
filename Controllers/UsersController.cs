using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiMovies.Models;
using ApiMovies.Models.Dtos;
using ApiMovies.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ApiMovies.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiUsers")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class UsersController: ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UsersController(IUserRepository userRepo, IMapper mapper, IConfiguration config)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _config = config;
        }

        /// <summary>
        /// Obtener lista de usuarios.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<UserDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetUsers()
        {
            var users = _userRepo.GetUsers();
            var usersDto = new List<UserDto>();
            foreach (var item in users)
            {
                usersDto.Add(_mapper.Map<UserDto>(item));
            }
            return Ok(usersDto);
        }

        /// <summary>
        /// Obtener usuario por id
        /// </summary>
        /// <param name="id">El id del usuario</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetUser")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetUser(int id)
        {
            var user = _userRepo.GetUser(id);
            if (user == null) {
                return NotFound();
            }
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        /// <summary>
        /// Registro de nuevo Usuario
        /// </summary>
        /// <param name="userAuthDto">El usuario que se va a registrar.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Registry")]
        [ProducesResponseType(201, Type = typeof(UserAuthDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Registry(UserAuthDto userAuthDto)
        {
            userAuthDto.User = userAuthDto.User.ToLower();
            if (_userRepo.UserExists(userAuthDto.User))
            {
                return BadRequest("El usuario ya existe");
            }

            var newUser = new User
            {
                UserA = userAuthDto.User
            };

            var createdUser = _userRepo.Registry(newUser, userAuthDto.Password);
            return Ok(createdUser);
        }

        /// <summary>
        /// Acceso/Autenticación de usuario
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserAuthLoginDto userAuthLoginDto)
        {
            //throw new Exception("Error generado");

            var user = _userRepo.Login(userAuthLoginDto.User, userAuthLoginDto.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            // Validación de claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserA.ToString())
            };

            // Generate Token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}