using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSImageDetails.Infra
{
    public interface ICommand : IRequest<CommandResult>
    {
    }

    public class CommandResult
    {
        public bool Success { get; set; }
    }
}
