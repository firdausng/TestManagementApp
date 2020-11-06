using AppCore.Common.Exceptions;
using AppCore.Common.Interfaces;
using AppCore.Domain.Entities.TestRepository;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Services.TestRepository.Queries.GetScenario
{
    public class GetScenarioQuery : IRequest<GetScenarioDto>
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public bool WithFeature { get; private set; }
        public GetScenarioQuery(Guid id, Guid projectId, bool withFeature = false)
        {
            Id = id;
            ProjectId = projectId;
            WithFeature = withFeature;
        }
        public class QueryHandler : IRequestHandler<GetScenarioQuery, GetScenarioDto>
        {
            private readonly IAppDbContext context;
            public QueryHandler(IAppDbContext context)
            {
                this.context = context;
            }
            public async Task<GetScenarioDto> Handle(GetScenarioQuery request, CancellationToken cancellationToken)
            {
                var entityQuery = context.Scenarios
                    .Include(s => s.Project)
                    .Where(s => s.Project.Id == request.ProjectId);

                if (request.WithFeature)
                {
                    entityQuery
                    .Include(s => s.Feature);
                }
                var entity = await entityQuery.FirstOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

                if (!entity.IsEntityExist())
                {
                    throw new EntityNotFoundException(nameof(Scenario), request.Id);
                }

                var dto = new GetScenarioDto
                {
                    Id = entity.Id,
                    ProjectId = entity.Project.Id,
                    Description = entity.Description,
                };

                if (request.WithFeature)
                {
                    dto.FeatureId = entity.Feature.Id;
                }

                return dto;
            }
        }

        public class GetScenarioQueryValidator : AbstractValidator<GetScenarioQuery>
        {
            public GetScenarioQueryValidator()
            {
                RuleFor(v => v.Id)
                    .NotEmpty()
                    .NotNull();
            }
        }
    }
}
