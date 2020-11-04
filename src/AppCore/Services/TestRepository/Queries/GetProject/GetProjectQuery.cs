using AppCore.Domain.Entities.TestRepository;
using AppCore.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Common.Exceptions;
using FluentValidation;

namespace AppCore.Services.TestRepository.Queries
{
    public class GetProjectQuery : IRequest<GetProjectDto>
    {
        public Guid Id { get; set; }
        public GetProjectQuery(Guid Id)
        {
            this.Id = Id;
        }
        public class QueryHandler : IRequestHandler<GetProjectQuery, GetProjectDto>
        {
            private readonly IAppDbContext context;
            public QueryHandler(IAppDbContext context)
            {
                this.context = context;
            }
            public async Task<GetProjectDto> Handle(GetProjectQuery request, CancellationToken cancellationToken)
            {
                var entitiy = await context.Projects
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

                if (entitiy == null)
                {
                    throw new EntityNotFoundException(nameof(Project), request.Id);
                }

                var dto = new GetProjectDto
                {
                    Id = entitiy.Id,
                    Name = entitiy.Name,
                    IsEnabled = entitiy.IsEnabled,
                };
                return dto;
            }
        }

        public class GetProjectQueryValidator : AbstractValidator<GetProjectQuery>
        {
            public GetProjectQueryValidator()
            {
                RuleFor(v => v.Id)
                    .NotEmpty()
                    .NotNull();
            }
        }
    }
}
