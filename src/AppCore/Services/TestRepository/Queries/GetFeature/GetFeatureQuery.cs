using AppCore.Common.Exceptions;
using AppCore.Common.Interfaces;
using AppCore.Domain.Entities.Common.Guards;
using AppCore.Domain.Entities.TestRepository;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Services.TestRepository.Queries.GetFeature
{
    public class GetFeatureQuery : IRequest<GetFeatureDto>
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public GetFeatureQuery(Guid Id, Guid ProjectId)
        {
            this.Id = Id;
            this.ProjectId = ProjectId;
        }
        public class QueryHandler : IRequestHandler<GetFeatureQuery, GetFeatureDto>
        {
            private readonly IAppDbContext context;
            public QueryHandler(IAppDbContext context)
            {
                this.context = context;
            }
            public async Task<GetFeatureDto> Handle(GetFeatureQuery request, CancellationToken cancellationToken)
            {
                var entity = await context.Features
                    .AsNoTracking()
                    .Include(f => f.Project)
                    .Where(f => f.Project.Id.Equals(request.ProjectId))
                    .Where(f => f.Id.Equals(request.Id))
                    .SingleOrDefaultAsync(cancellationToken);

                EntityGuard.NullGuard(entity, new EntityNotFoundException(nameof(Feature), request.ProjectId));

                var dto = new GetFeatureDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    ProjectId = entity.Project.Id
                };

                return dto;
            }
        }

        public class GetFeatureQueryValidator : AbstractValidator<GetFeatureQuery>
        {
            public GetFeatureQueryValidator()
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
