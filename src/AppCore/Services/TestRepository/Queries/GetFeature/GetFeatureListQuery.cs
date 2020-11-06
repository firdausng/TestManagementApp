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

namespace AppCore.Services.TestRepository.Queries.GetFeature
{
    public class GetFeatureListQuery : IRequest<GetObjectListDto<GetFeatureDto>>
    {
        public Guid ProjectId { get; private set; }
        
        public GetFeatureListQuery(Guid ProjectId)
        {
            this.ProjectId = ProjectId;
        }

        public class QueryHandler : IRequestHandler<GetFeatureListQuery, GetObjectListDto<GetFeatureDto>>
        {
            private readonly IAppDbContext context;
            public QueryHandler(IAppDbContext context)
            {
                this.context = context;
            }
            public async Task<GetObjectListDto<GetFeatureDto>> Handle(GetFeatureListQuery request, CancellationToken cancellationToken)
            {
                var entitiesList = await context.Features
                    .AsNoTracking()
                    .Include(f => f.Project)
                    .Where(f => f.Project.Id.Equals(request.ProjectId))
                    .ToListAsync(cancellationToken);

                var list = entitiesList
                    .Select(x => new GetFeatureDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ProjectId = x.Project.Id,
                        Description = x.Description
                    }).ToList();

                var dto = new GetObjectListDto<GetFeatureDto>(list, list.Count);

                return dto;
            }
        }

        public class GetFeatureListQueryValidator : AbstractValidator<GetFeatureListQuery>
        {
            public GetFeatureListQueryValidator()
            {
                RuleFor(v => v.ProjectId)
                    .NotEmpty()
                    .NotNull();
            }
        }
    }
}
