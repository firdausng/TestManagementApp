using AppCore.Common.Exceptions;
using AppCore.Common.Interfaces;
using AppCore.Domain.Entities.TestRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public class DeleteFeatureItemCommandHandler : IRequestHandler<DeleteFeatureCommand>
        {
            private readonly IAppDbContext context;
            public DeleteFeatureItemCommandHandler(IAppDbContext context)
            {
                this.context = context;
            }

            public async Task<Unit> Handle(DeleteFeatureCommand request, CancellationToken cancellationToken)
            {
                var projectEntity = await context.Projects
                    .Where(t => t.Id == request.ProjectId)
                    .SingleOrDefaultAsync();

                if (!projectEntity.IsEntityExist())
                {
                    throw new EntityNotFoundException(nameof(Project), request.Id);
                }

                var featureEntity = await context.Features
                    .Where(t => t.Id == request.Id)
                    .SingleOrDefaultAsync();

                if (!featureEntity.IsEntityExist())
                {
                    throw new EntityNotFoundException(nameof(Feature), request.Id);
                }

                context.Features.Remove(featureEntity);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
