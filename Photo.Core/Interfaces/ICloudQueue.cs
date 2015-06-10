using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photo.Core.Models;

namespace Photo.Core.Interfaces
{
    public interface ICloudQueue
    {
        Task EnqueueMessage(MemeRequest message);
        Task<MemeRequest> DequeueMessage();
        event EventHandler NewMessageArrived;
    }
}
