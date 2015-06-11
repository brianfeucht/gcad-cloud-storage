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
        int Count { get; }
        Task EnqueueMessage(MemeRequest message);
        Task DequeueMessage(Func<MemeRequest, Task> processRequest);
    }
}
