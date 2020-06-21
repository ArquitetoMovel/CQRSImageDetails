using CQRSImageDetails.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSImageDetails.Events
{
    public class ImageCreatedEvent : IEvent
    {
        public string Name { get; set; }
    }
}
