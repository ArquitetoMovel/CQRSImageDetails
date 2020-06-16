using System.Threading;
using System.Threading.Tasks;
using CQRSImageDetails.Infra;
using CQRSImageDetails.Repository;

namespace CQRSImageDetails.Commands
{
    public class CreateNewImageCommandHandler : CommandHandler<CreateNewImageCommand>
    {
        private readonly RepositoryPostgres _repository;

        public CreateNewImageCommandHandler()
        {
            _repository = new RepositoryPostgres();
        }

        public override Task<CommandResult> Handle(CreateNewImageCommand request, CancellationToken cancellationToken) =>
        Task.Run<CommandResult>(() =>
        {
            var result = new CommandResult
            {
                Success = _repository.InsertImageDetails(request)
            };
            return result;
        });

       
    }
}
