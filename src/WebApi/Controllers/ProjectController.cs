using AppCore.Services.Common.Models;
using AppCore.Services.TestRepository.Commands;
using AppCore.Services.TestRepository.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/project")]
    public class ProjectController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProjectController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Name = nameof(GetProjects))]
        public async Task<ActionResult<GetObjectListDto<GetProjectDto>>> GetProjects()
        {
            var vm = await mediator.Send(new GetProjectListQuery());
            return Ok(vm);
        }

        [HttpGet("{projectId}", Name = nameof(GetProject))]
        public async Task<ActionResult<GetProjectDto>> GetProject(Guid projectId)
        {
            var vm = await mediator.Send(new GetProjectQuery(projectId));

            return Ok(vm);
        }

        [HttpPost(Name = nameof(NewProject))]
        public async Task<ActionResult<Guid>> NewProject(CreateProjectCommand createProjectCommand)
        {
            var vm = await mediator.Send(createProjectCommand);
            if (vm.Id != null)
            {
                var link = Url.Link(nameof(GetProject), new { projectId = vm.Id });
                return Created(link, vm);
            }
            else
            {
                return BadRequest(vm);
            }

        }

        [HttpDelete("{id}", Name = nameof(DeleteProject))]
        public async Task<ActionResult> DeleteProject(Guid id)
        {
            await mediator.Send(new DeleteProjectCommand(id));
            return NoContent();
        }
    }
}
