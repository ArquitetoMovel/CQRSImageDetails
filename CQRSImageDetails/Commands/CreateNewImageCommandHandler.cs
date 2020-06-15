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
    public class CreateNewImageCommandHandler : IRequestHandler<CreateNewImageCommand, bool>
    {
        private readonly RepositoryPostgres _repository;

        public CreateNewImageCommandHandler()
        {
            _repository = new RepositoryPostgres();
        }

        public Task<bool> Handle(CreateNewImageCommand request, CancellationToken cancellationToken) =>
        Task.Run<bool>(() =>
        {
          return _repository.InsertImageDetails(request);
        });
        
    }
}
