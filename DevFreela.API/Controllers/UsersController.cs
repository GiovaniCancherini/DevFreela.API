using DevFreela.Application.Models;
using DevFreela.Application.Services;
using DevFreela.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get(string search = "")
        {
            var result = _service.GetAll(search);

            if (!result.IsSucess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _service.GetById(id);

            if (!result.IsSucess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post(CreateUserInputModel model)
        {
            var result = _service.Insert(model);

            if (!result.IsSucess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPost("{id}/skills")]
        public IActionResult PostSkills(int id, UserSkillInputModel model)
        {
            var result = _service.InsertSkills(id, model);

            if (!result.IsSucess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPut("{id}/profile-picture")]
        public IActionResult PostProfilePicture(int id, IFormFile file)
        {
            var result = _service.InsertProfilePicture(id, file);

            if (!result.IsSucess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPut("{id}/login")]
        public IActionResult Login(int id, LoginModel login)
        {
            var result = _service.Login(id, login);

            if (!result.IsSucess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
    }
}