using ExeCutor;

namespace CQRSImageDetails.Commands
{
    public class RemoveImageCommand : ICommand
    {
        public int Id { get; set; }
    }
}
