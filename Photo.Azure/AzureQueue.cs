using Photo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photo.Core.Models;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using Newtonsoft.Json;

namespace Photo.Azure
{
    public class AzureQueue : ICloudQueue
    {
        private readonly string queueName;

        public int Count
        {
            get
            {
                var value = Queue.ApproximateMessageCount;

                return value ?? 0;
            }
        }

        public CloudQueue Queue
        {
            get
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                                    ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

                var queueClient = storageAccount.CreateCloudQueueClient();
                return queueClient.GetQueueReference("myqueue");
            }
        }

        public AzureQueue(string queueName)
        {
            this.queueName = queueName;
        }

        public async Task EnsureQueueHasBeenCreated()
        {
            await Queue.CreateIfNotExistsAsync();
        }

        public async Task DequeueMessage(Func<MemeRequest, Task> processRequest)
        {
            var message = await Queue.GetMessageAsync();
            var request = JsonConvert.DeserializeObject<MemeRequest>(message.AsString);

            await processRequest(request);

            await Queue.DeleteMessageAsync(message);
        }

        public async Task EnqueueMessage(MemeRequest request)
        {
            var content = JsonConvert.SerializeObject(request);
            var message = new CloudQueueMessage(content);
            await Queue.AddMessageAsync(message);
        }

        public async Task Clear()
        {
            await Queue.ClearAsync();
        }
    }
}
