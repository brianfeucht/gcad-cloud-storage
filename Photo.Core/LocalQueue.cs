using Nito.AsyncEx;
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
        private readonly ConcurrentQueue<MemeRequest> queue;
        private readonly AsyncMonitor monitor;

        public int Count
        {
            get
            {
                return queue.Count;
            }
        }

        public LocalQueue()
        {
            queue = new ConcurrentQueue<MemeRequest>();
            monitor = new AsyncMonitor();
        }

        public async Task EnqueueMessage(MemeRequest message)
        {
            queue.Enqueue(message);

            using (await monitor.EnterAsync())
            {
                monitor.PulseAll();
            }
        }

        public async Task DequeueMessage(Func<MemeRequest, Task> processRequest)
        {
            MemeRequest request;

            while (!queue.TryDequeue(out request))
            {
                using (await monitor.EnterAsync())
                {
                    await monitor.WaitAsync();
                }
            }

            await processRequest(request);
        }
    }
}
