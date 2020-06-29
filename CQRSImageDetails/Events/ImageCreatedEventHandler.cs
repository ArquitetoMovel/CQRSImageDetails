using System;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSImageDetails.Events
{
    public class ImageCreatedEventHandler : ExeCutor.EventHandler<ImageCreatedEvent>
    {
        public override Task HandleExecution(ImageCreatedEvent @event, CancellationToken cancellationToken) =>
        Task.Run(() =>
        {
            Console.WriteLine($"I GO TO PUBLISH {@event.Name} ON MESSAGE BROKER IN {DateTime.Now}");
        });
    }
}
