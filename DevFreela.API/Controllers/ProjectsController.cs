using DevFreela.API.Persistence;
using DevFreela.API.Services;
using DevFreela.Application.Models;
using DevFreela.Application.Services;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevFreela.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly DevFreelaDbContext _context;
        private readonly FreelanceTotalCostConfig _config;
        private readonly IConfigService _configServic;
        private readonly IProjectService _service;

        public ProjectsController(
            DevFreelaDbContext context,
            IOptions<FreelanceTotalCostConfig> option,
            IConfigService configService,
            IProjectService service)
        {
            _config = option.Value;
            _configServic = configService;

            _context = context;
            _service = service;
        }

        [HttpGet]
        public IActionResult Get(string search = "", int page = 0, int size = 3)
        {
            var result = _service.GetAll(search, page, size);

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
        public IActionResult Post(CreateProjectInputModel model)
        {
            var result = _service.Insert(model);

            if (!result.IsSucess)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data }, model);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateProjectInputModel model)
        {
            var result = _service.Update(id, model);

            if (!result.IsSucess)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _service.Delete(id);

            if (!result.IsSucess)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

        [HttpPut("{id}/start")]
        public IActionResult Start(int id)
        {
            var result = _service.Start(id);

            if (!result.IsSucess)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

        [HttpPut("{id}/cancel")]
        public IActionResult Cancel(int id)
        {
            var result = _service.Cancel(id);

            if (!result.IsSucess)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

        [HttpPut("{id}/complete")]
        public IActionResult Complete(int id)
        {
            var result = _service.Complete(id);

            if (!result.IsSucess)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

        [HttpPost("{id}/comments")]
        public IActionResult PostComment(int id, CreateCommentInputModel createComment)
        {
            var result = _service.InsertComment(id, createComment);

            if (!result.IsSucess)
            {
                return BadRequest(result.Message);
            }

            // ? -> return Ok(result.Data);
            return NoContent();
        }
    }
}

