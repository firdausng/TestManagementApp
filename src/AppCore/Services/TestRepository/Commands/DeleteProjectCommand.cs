using AppCore.Common.Exceptions;
using AppCore.Common.Interfaces;
using AppCore.Domain.Entities.TestRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Services.TestRepository.Commands
{
    public class DeleteProjectCommand: IRequest
    {
        public Guid Id { get; set; }
        public DeleteProjectCommand(Guid Id)
        {
            this.Id = Id;
        }
        public class DeleteProjectItemCommandHandler : IRequestHandler<DeleteProjectCommand>
        {
            private readonly IAppDbContext context;
            public DeleteProjectItemCommandHandler(IAppDbContext context)
            {
                this.context = context;
            }

            public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
            {
                var project = await context.Projects
                    .Where(t => t.Id == request.Id)
                    .SingleOrDefaultAsync();

                if (project == null)
                {
                    throw new EntityNotFoundException(nameof(Project), request.Id);
                }

                context.Projects.Remove(project);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
