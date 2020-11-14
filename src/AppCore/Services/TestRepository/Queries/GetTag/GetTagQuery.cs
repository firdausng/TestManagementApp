using AppCore.Common.Exceptions;
using AppCore.Common.Interfaces;
using AppCore.Domain.Entities.Common.Guards;
using AppCore.Domain.Entities.TestRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

namespace AppCore.Services.TestRepository.Queries.GetTag
{
    public class GetTagQuery : IRequest<GetTagDto>
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public GetTagQuery(Guid Id, Guid ProjectId)
        {
            this.Id = Id;
            this.ProjectId = ProjectId;
        }
        public class QueryHandler : IRequestHandler<GetTagQuery, GetTagDto>
        {
            private readonly IAppDbContext context;
            public QueryHandler(IAppDbContext context)
            {
                this.context = context;
            }
            public async Task<GetTagDto> Handle(GetTagQuery request, CancellationToken cancellationToken)
            {
                var entity = await context.Tags
                    .AsNoTracking()
                    .Include(f => f.Project)
                    .Include(f => f.FeatureList)
                    .Include(f => f.ScenarioList)
                    .Where(f => f.Project.Id.Equals(request.ProjectId))
                    .Where(f => f.Id.Equals(request.Id))
                    .SingleOrDefaultAsync(cancellationToken);

                EntityGuard.NullGuard(entity, new EntityNotFoundException(nameof(Tag), request.Id));

                var dto = new GetTagDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    ProjectId = entity.Project.Id,
                    FeatureList = entity.FeatureList.Select(f => f.Id).ToList(),
                    ScenarioList = entity.ScenarioList.Select(s => s.Id).ToList(),
                };

                return dto;
            }
        }

        public class GetTagQueryValidator : AbstractValidator<GetTagQuery>
        {
            public GetTagQueryValidator()
            {
                RuleFor(v => v.Id)
                    .NotEmpty()
                    .NotNull();

                RuleFor(v => v.ProjectId)
                    .NotEmpty()
                    .NotNull();
            }
        }
    }
}
