using AppCore.Common.Exceptions;
using AppCore.Common.Interfaces;
using AppCore.Domain.Entities.TestRepository;
using AppCore.Services.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Services.TestRepository.Queries.GetScenario
{
    public class GetScenarioListQuery : IRequest<GetObjectListDto<GetScenarioDto>>
    {
        public Guid ProjectId { get; private set; }
        public bool WithFeature { get; private set; }

        public GetScenarioListQuery(Guid projectId, bool withFeature)
        {
            ProjectId = projectId;
            WithFeature = withFeature;
        }

        public class QueryHandler : IRequestHandler<GetScenarioListQuery, GetObjectListDto<GetScenarioDto>>
        {
            private readonly IAppDbContext context;
            public QueryHandler(IAppDbContext context)
            {
                this.context = context;
            }
            public async Task<GetObjectListDto<GetScenarioDto>> Handle(GetScenarioListQuery request, CancellationToken cancellationToken)
            {
                var entityListQuery = context.Scenarios
                    .Include(s => s.Project)
                    .Where(s => s.Project.Id == request.ProjectId);

                if (request.WithFeature)
                {
                    entityListQuery
                    .Include(s => s.Feature);
                }

                var entitiesList = await entityListQuery.ToListAsync(cancellationToken);

                var list = entitiesList
                    .Select(x => 
                    {
                        var dto = new GetScenarioDto
                        {
                            Id = x.Id,
                            ProjectId = x.Project.Id,
                            Description = x.Description
                        };

                        if (request.WithFeature)
                        {
                            dto.FeatureId = x.Feature.Id;
                        }

                        return dto;
                    }).ToList();

                var dto = new GetObjectListDto<GetScenarioDto>(list, list.Count);

                return dto;
            }
        }

        public class GetScenarioListQueryValidator : AbstractValidator<GetScenarioListQuery>
        {
            public GetScenarioListQueryValidator()
            {
                RuleFor(v => v.ProjectId)
                    .NotEmpty()
                    .NotNull();
            }
        }
    }
}
