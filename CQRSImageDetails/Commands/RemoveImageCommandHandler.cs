using MediatR;
using CQRSImageDetails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSImageDetails.Commands
{
    public class RemoveImageCommandHandler : IRequestHandler<RemoveImageCommand, bool>
    {
        private readonly RepositoryPostgres _repository;

        public RemoveImageCommandHandler()
        {
            _repository = new RepositoryPostgres();
        }

        public Task<bool> Handle(RemoveImageCommand request, CancellationToken cancellationToken) =>
        Task.Run<bool>(() =>
        {
            return _repository.DeleteImageDetails(request.Id);
        });

    }
}
