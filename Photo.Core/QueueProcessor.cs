using Nito.AsyncEx;
using Photo.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Photo.Core
{
    public class QueueProcessor
    {
        private readonly IMemeGenerator generator;
        private readonly ICloudQueue queue;

        private Task runTask;
        private bool running;

        public QueueProcessor(ICloudQueue queue, IMemeGenerator generator)
        {
            this.queue = queue;
            this.generator = generator;
        }

        private object startSyncObj = new object();
        public void Start()
        {
            if (running)
                return;

            lock (startSyncObj)
            {
                if (running)
                    return;

                running = true;
                runTask = Run();
            }
        }

        private async Task Run()
        {
            while (running)
            {
                try
                {
                    await queue.DequeueMessage(memeRequest => generator.GenerateMeme(memeRequest));
                }
                catch(Exception ex)
                {

                }
            }
        }

        public void Stop()
        {
            running = false;
        }
    }
}
