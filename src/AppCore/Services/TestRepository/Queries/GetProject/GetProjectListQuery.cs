using AppCore.Common.Interfaces;
using AppCore.Services.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Services.TestRepository.Queries
{
    public class GetProjectListQuery : IRequest<GetObjectListDto<GetProjectDto>>
    {
        public class QueryHandler : IRequestHandler<GetProjectListQuery, GetObjectListDto<GetProjectDto>>
        {
            private readonly IAppDbContext context;
            public QueryHandler(IAppDbContext context)
            {
                this.context = context;
            }
            public async Task<GetObjectListDto<GetProjectDto>> Handle(GetProjectListQuery request, CancellationToken cancellationToken)
            {
                var entitiesList = await context.Projects
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                var list = entitiesList
                    .Select(x => new GetProjectDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IsEnabled = x.IsEnabled,
                    }).ToList();

                var dto = new GetObjectListDto<GetProjectDto>(list, list.Count);

                return dto;
            }
        }
    }
}
