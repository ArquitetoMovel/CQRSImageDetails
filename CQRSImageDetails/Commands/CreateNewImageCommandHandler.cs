using MediatR;
using CQRSImageDetails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatrSampleDB.Infra;

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
