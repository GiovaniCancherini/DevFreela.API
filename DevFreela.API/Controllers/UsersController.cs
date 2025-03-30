using DevFreela.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevFreela.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok();
        }

        [HttpPost] // POST api/users
        public IActionResult Post(CreateUserModel createUser)
        {
            return CreatedAtAction(nameof(GetById), new { id = 1 }, createUser);
        }

        [HttpPost("{id}/skills")]
        public IActionResult PostSkills(UserSkillInputModel model)
        {
            return NoContent();
        }

        [HttpPut("{id}/login")]
        public IActionResult Login(int id, LoginModel login)
        {
            return NoContent();
        }

        [HttpPut("{id}/profile-picture")]
        public IActionResult PostProfilePicture(IFormFile file)
        {
            var description = $"File: {file.FileName}, Size: {file.Length}";

            // processar imagem

            return Ok(description);
        }

        [HttpPost]
        public IActionResult Post(CreateUserInputModel model)
        {
            return Ok();
        }
    }
}