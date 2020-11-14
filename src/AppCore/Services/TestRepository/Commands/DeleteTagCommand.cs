using AppCore.Domain.Entities.TestRepository;
using AppCore.Common.Interfaces;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using AppCore.Domain.Entities.Common.Guards;
using AppCore.Common.Exceptions;
using System.Linq;

namespace AppCore.Services.TestRepository.Commands
{
    public class DeleteTagCommand : IRequest
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public DeleteTagCommand(Guid Id, Guid ProjectId)
        {
            this.Id = Id;
            this.ProjectId = ProjectId;
        }
        public class Handler : IRequestHandler<DeleteTagCommand>
        {
            private readonly IAppDbContext context;
            public Handler(IAppDbContext context)
            {
                this.context = context;
            }

            public async Task<Unit> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
            {
                var tagEntity = await context.Tags
                    .Include(t => t.Project)
                    .Where(t => t.Project.Id == request.ProjectId)
                    .Where(t => t.Id == request.Id)
                    .SingleOrDefaultAsync();

                EntityGuard.NullGuard(tagEntity, new EntityNotFoundException(nameof(Project), request.Id));

                context.Tags.Remove(tagEntity);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
