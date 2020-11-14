using AppCore.Common.Interfaces;
using AppCore.Services.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Services.TestRepository.Queries.GetTag
{
    public class GetTagListQuery : IRequest<GetObjectListDto<GetTagDto>>
    {
        public Guid ProjectId { get; private set; }

        public GetTagListQuery(Guid ProjectId)
        {
            this.ProjectId = ProjectId;
        }

        public class QueryHandler : IRequestHandler<GetTagListQuery, GetObjectListDto<GetTagDto>>
        {
            private readonly IAppDbContext context;
            public QueryHandler(IAppDbContext context)
            {
                this.context = context;
            }
            public async Task<GetObjectListDto<GetTagDto>> Handle(GetTagListQuery request, CancellationToken cancellationToken)
            {
                var entitiesList = await context.Tags
                    .AsNoTracking()
                    .Include(f => f.Project)
                    .Where(f => f.Project.Id.Equals(request.ProjectId))
                    .ToListAsync(cancellationToken);

                var list = entitiesList
                    .Select(x => 
                    {
                        var tagDto = new GetTagDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            ProjectId = x.Project.Id,
                            Description = x.Description,
                        };
                        return tagDto;
                    }).ToList();

                var dto = new GetObjectListDto<GetTagDto>(list, list.Count);

                return dto;
            }
        }

        public class GetTagListQueryValidator : AbstractValidator<GetTagListQuery>
        {
            public GetTagListQueryValidator()
            {
                RuleFor(v => v.ProjectId)
                    .NotEmpty()
                    .NotNull();

            }
        }
    }
}
