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
        
        private readonly AsyncMonitor monitor;

        private Task runTask;
        private bool running;

        public QueueProcessor(ICloudQueue queue, IMemeGenerator generator)
        {
            this.queue = queue;
            this.generator = generator;

            this.monitor = new AsyncMonitor();
            this.queue.NewMessageArrived += Queue_NewMessageArrived;
        }

        private async void Queue_NewMessageArrived(object sender, EventArgs e)
        {
            await NotifyOfNewMessage();
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
                var memeRequest = await queue.DequeueMessage();

                if (memeRequest == null)
                {
                    try
                    {
                        var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));

                        using (await monitor.EnterAsync(cts.Token))
                        {
                            await monitor.WaitAsync(cts.Token);
                        }

                    }
                    catch (TaskCanceledException) { }
                }
                else
                {
                    await generator.GenerateMeme(memeRequest);
                }
            }
        }

        public void Stop()
        {
            running = false;

            using (monitor.Enter())
            {
                monitor.PulseAll();
            }

            if (runTask != null && runTask.IsCanceled)
                runTask.Wait();
        }

        public async Task NotifyOfNewMessage()
        {
            if (!running)
                Start();

            using (await monitor.EnterAsync())
            {
                monitor.PulseAll();
            }
        }
    }
}
