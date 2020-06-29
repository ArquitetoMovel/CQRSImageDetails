using ExeCutor;

namespace CQRSImageDetails.Commands
{
    public class CreateNewImageCommand : ICommand<CreateNewImageResult>
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }

    public class CreateNewImageResult : ICommandResponse
    {
        public int Id { get; set; }
    }
}
