using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSImageDetails.Infra
{
    public abstract class CommandHandler<T> : IRequestHandler<T, CommandResult> 
        where T : ICommand
    {

        Task<CommandResult> IRequestHandler<T, CommandResult>.Handle(T request, CancellationToken cancellationToken) =>
        Handle(request, cancellationToken);
       
        public abstract Task<CommandResult> Handle(T command, CancellationToken cancellationToken);
        
    }
}
