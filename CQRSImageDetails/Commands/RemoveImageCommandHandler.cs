using CQRSImageDetails.Repository;
using ExeCutor;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSImageDetails.Commands
{
    public class RemoveImageCommandHandler : CommandHandler<RemoveImageCommand>
    {
        private readonly RepositoryPostgres _repository;

        public RemoveImageCommandHandler()
        {
            _repository = new RepositoryPostgres();
        }

        public override Task<CommandResponse> HandleExecution(RemoveImageCommand command, CancellationToken cancellationToken) =>
        Task.Run<CommandResponse>(() =>
        {
            return new CommandResponse
            {
                OK = _repository.DeleteImageDetails(command.Id)
            };
        });
    }
}
