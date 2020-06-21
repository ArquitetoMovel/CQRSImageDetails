using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSImageDetails.Infra
{
    public abstract class IntegrationEventHandler<T> : INotificationHandler<T> where T : IEvent
    {
        public Task Handle(T @event, CancellationToken cancellationToken) => HandleIntegration(@event, cancellationToken);

        public abstract Task HandleIntegration(T @event, CancellationToken cancellationToken);
    }
}
