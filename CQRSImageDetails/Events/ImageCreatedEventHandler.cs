using CQRSImageDetails.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSImageDetails.Events
{
    public class ImageCreatedEventHandler : IntegrationEventHandler<ImageCreatedEvent>
    {
        public override Task HandleIntegration(ImageCreatedEvent @event, CancellationToken cancellationToken) =>
        Task.Run(() =>
        {
           Console.WriteLine($"I GO TO PUBLISH {@event.Name} ON MESSAGE BROKER IN {DateTime.Now}");
        });

    }
}
