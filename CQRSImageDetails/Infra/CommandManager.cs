using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSImageDetails.Infra
{
    public class CommandManager
    {
        private readonly IMediator _mediator;

        public CommandManager(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<CommandResult> Send(ICommand command) => _mediator.Send<CommandResult>(command);
    }
}
