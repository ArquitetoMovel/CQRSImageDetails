using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediatrSampleDB.Infra
{
    public class CommandEngine
    {
        private readonly IMediator _mediator;

        public CommandEngine(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<CommandResult> Send(ICommand command)
        {
            return _mediator.Send<CommandResult>(command);
        }
    }
}
