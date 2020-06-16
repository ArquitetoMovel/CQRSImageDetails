using CQRSImageDetails.Infra;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSImageDetails.Commands
{
    public class RemoveImageCommand : ICommand
    {
        public int Id { get; set; }
    }
}
