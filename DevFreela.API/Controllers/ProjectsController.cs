using DevFreela.API.Models;
using DevFreela.API.Services;
using Microsoft.AspNetCore.Mvc;
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
        private readonly FreelanceTotalCostConfig _config;
        private readonly IConfigService _configServic;

        public ProjectsController(
            IOptions<FreelanceTotalCostConfig> option,
            IConfigService configService)
        {
            _config = option.Value;
            _configServic = configService;
        }


        // GET: api/projects?search=abc
        [HttpGet]
        public IActionResult Get(string search = "")
        {
            //return Ok(_configServic.GetValue());
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok();
        }

        // POST: api/projects
        [HttpPost]
        public IActionResult Post(CreateProjectModel createProject)
        {
            if (createProject.Title.Length > 50)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetById), new { id = createProject.IdClient}, createProject);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateProjectModel updateProject)
        {
            updateProject.IdProject = id;

            if (updateProject.Description.Length > 200)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            
            return NoContent();
        }

        [HttpPut("{id}/start")]
        public IActionResult Start(int id)
        {
            return NoContent();
        }

        [HttpPut("{id}/complete")]
        public IActionResult Complete(int id)
        {
            return NoContent();
        }

        [HttpPost("{id}/comments")]
        public IActionResult PostComment(int id, CreateCommentModel createComment)
        {
            return NoContent(); // CreatedAtAction(nameof(GetById), new { id = id }, createComment);   
        }
    }
}

