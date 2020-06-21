using MediatR;

namespace CQRSImageDetails.Infra
{
    public interface ICommand : IRequest<CommandResult> { }

    public class CommandResult 
    {
        public int Id { get; set; }
        public  bool Success { get; set; }
    }
        
}
