using AppCore.Services.Common.Models;
using AppCore.Services.TestRepository.Commands;
using AppCore.Services.TestRepository.Queries.GetFeature;
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
        public async Task<ActionResult<GetObjectListDto<GetFeatureDto>>> GetProjectFeatures(Guid projectId)
        {
            var vm = await mediator.Send(new GetFeatureListQuery(projectId));
            return Ok(vm);
        }

        [HttpGet("{featureId}", Name = nameof(GetProjectFeature))]
        public async Task<ActionResult<GetFeatureDto>> GetProjectFeature(Guid featureId, Guid projectId)
        {
            var vm = await mediator.Send(new GetFeatureQuery(featureId, projectId));
            return Ok(vm);
        }

        [HttpPost(Name = nameof(NewFeature))]
        public async Task<ActionResult<Guid>> NewFeature(CreateFeatureCommand createFeatureCommand)
        {
            var vm = await mediator.Send(createFeatureCommand);
            if (vm.Id != Guid.Empty)
            {
                var link = Url.Link(nameof(GetProjectFeature), new { featureId = vm.Id });
                return Created(link, vm);
            }
            else
            {
                return BadRequest(vm);
            }

        }

        [HttpDelete("{id}", Name = nameof(DeleteFeature))]
        public async Task<ActionResult> DeleteFeature(Guid id, Guid projectId)
        {
            await mediator.Send(new DeleteFeatureCommand(id, projectId));
            return NoContent();
        }
    }
}
