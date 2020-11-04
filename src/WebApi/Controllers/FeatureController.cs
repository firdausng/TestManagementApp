using AppCore.Services.TestRepository.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/project/{projectId}/feature")]
    [ApiController]
    public class FeatureController : ControllerBase
    {
        private readonly IMediator mediator;

        public FeatureController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Name = nameof(GetProjectFeatures))]
        //public async Task<ActionResult<GetObjectListVm<GetProjectFeatureListDto>>> GetProjectFeatures(Guid projectId)
        public async Task<IActionResult> GetProjectFeatures(Guid projectId)
        {
            //var vm = await mediator.Send(new GetProjectFeatureListQuery() { ProjectId = projectId });
            //return Ok(vm);
            return Ok();
        }

        [HttpGet("{featureId}", Name = nameof(GetProjectFeature))]
        //public async Task<ActionResult<GetProjectFeatureListDto>> GetProjectFeature(Guid featureId, Guid projectId)
        public async Task<IActionResult> GetProjectFeature(Guid featureId, Guid projectId)
        {
            //var vm = await mediator.Send(new GetProjectFeatureItemQuery() { Id = featureId, ProjectId = projectId });

            //return Ok(vm);
            return Ok();
        }

        [HttpPost(Name = nameof(NewFeature))]
        public async Task<ActionResult<Guid>> NewFeature(CreateFeatureCommand createFeatureCommand)
        {
            var vm = await mediator.Send(createFeatureCommand);
            if (vm.Id != null)
            {
                var link = Url.Link(nameof(GetProjectFeature), new { featureId = vm.Id });
                return Created(link, vm);
            }
            else
            {
                return BadRequest(vm);
            }

        }
    }
}
