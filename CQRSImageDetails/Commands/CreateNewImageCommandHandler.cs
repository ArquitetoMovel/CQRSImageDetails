using System;
using System.Threading;
using System.Threading.Tasks;
using CQRSImageDetails.Repository;
using ExeCutor;

namespace CQRSImageDetails.Commands
{
    public class CreateNewImageCommandHandler : CommandHandler<CreateNewImageCommand, CreateNewImageResult>
    {
        private readonly RepositoryPostgres _repository;

        public CreateNewImageCommandHandler()
        {
            _repository = new RepositoryPostgres();
        }

        public override Task<CreateNewImageResult> Handle(CreateNewImageCommand request, CancellationToken cancellationToken) =>
        Task.Run<CreateNewImageResult>(() =>
        {
            Console.WriteLine($"I GO TO CREATE  {request.Name} IN {DateTime.Now}");
            var result = new CreateNewImageResult
            {
                Id = _repository.InsertImageDetails(request) ? _repository.ImageID : 0
            };
            return result;
        });      
    }
}
