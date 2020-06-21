using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSImageDetails.Infra
{
    public class EventManager
    {
        private readonly IMediator _mediator;

        public EventManager(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Publish(IEvent @event) =>  _mediator.Publish(@event);
    }
}
