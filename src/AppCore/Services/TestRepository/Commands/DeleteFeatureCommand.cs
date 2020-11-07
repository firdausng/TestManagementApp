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

namespace AppCore.Services.TestRepository.Commands
{
    public class DeleteFeatureCommand : IRequest
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public DeleteFeatureCommand(Guid Id, Guid ProjectId)
        {
            this.Id = Id;
            this.ProjectId = ProjectId;
        }
        public class Handler : IRequestHandler<DeleteFeatureCommand>
        {
            private readonly IAppDbContext context;
            public Handler(IAppDbContext context)
            {
                this.context = context;
            }

            public async Task<Unit> Handle(DeleteFeatureCommand request, CancellationToken cancellationToken)
            {
                var featureEntity = await context.Features
                    .Include(t => t.Project)
                    .Where(t => t.Project.Id == request.ProjectId)
                    .Where(t => t.Id == request.Id)
                    .SingleOrDefaultAsync();

                EntityGuard.NullGuard(featureEntity, new EntityNotFoundException(nameof(Project), request.Id));

                context.Features.Remove(featureEntity);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
