using MediatR;
using CQRSImageDetails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CQRSImageDetails.Infra;

namespace CQRSImageDetails.Commands
{
    public class RemoveImageCommandHandler : CommandHandler<RemoveImageCommand>
    {
        private readonly RepositoryPostgres _repository;

        public RemoveImageCommandHandler()
        {
            _repository = new RepositoryPostgres();
        }

        public override Task<CommandResult> Handle(RemoveImageCommand request, CancellationToken cancellationToken) =>
        Task.Run<CommandResult>(() =>
        {
            return new CommandResult
            {
                Success = _repository.DeleteImageDetails(request.Id)
            };
        });

    }
}
