using MediatR;
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

        public Task<R> Send<R>(ICommand<R> command) where R : IResult
        {
            return _mediator.Send<R>(command);
        }
    }
}
