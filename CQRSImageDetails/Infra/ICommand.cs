using MediatR;

namespace CQRSImageDetails.Infra
{
    public interface ICommand : IRequest<CommandResult> { }
    
    public interface ICommand<R> : IRequest<R> where R : IResult { }
    
    public interface IResult { }

    public class CommandResult : IResult
    {
        public  bool Success { get; set; }
    }        
}
