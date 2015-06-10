using Photo.Core.Interfaces;
using Photo.Core.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Core
{
    public class LocalQueue : ICloudQueue
    {
        internal readonly ConcurrentQueue<MemeRequest> queue;
        public event EventHandler NewMessageArrived = delegate { };

        public LocalQueue()
        {
            queue = new ConcurrentQueue<MemeRequest>();
        }

        public Task EnqueueMessage(MemeRequest message)
        {
            queue.Enqueue(message);
            NewMessageArrived.Invoke(this, null);
            return Task.FromResult(0);
        }

        public Task<MemeRequest> DequeueMessage()
        {
            MemeRequest request;
            queue.TryDequeue(out request);
            return Task.FromResult(request);
        }
    }
}
