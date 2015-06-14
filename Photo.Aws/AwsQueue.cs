using Photo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photo.Core.Models;
using Amazon.SQS;
using Amazon;
using Amazon.SQS.Model;
using Newtonsoft.Json;

namespace Photo.Aws
{
    public class AwsQueue : ICloudQueue
    {
        private readonly AmazonSQSClient queueClient;
        private readonly string queueUrl;

        public int Count
        {
            get
            {
                return queueClient.GetQueueAttributes(queueUrl, new[] { "ApproximateNumberOfMessages" }.ToList()).ApproximateNumberOfMessages;
            }
        }

        public AwsQueue(string queueName)
        {
            var credentialFactory = new AwsCredentialFactory();

            queueClient = new AmazonSQSClient(credentialFactory.Credentials(), RegionEndpoint.USWest2);
            queueUrl = queueClient.GetQueueUrl(new GetQueueUrlRequest(queueName)).QueueUrl;
        }

        public async Task DequeueMessage(Func<MemeRequest, Task> processRequest)
        {
            var response = await queueClient.ReceiveMessageAsync(new ReceiveMessageRequest()
            {
                QueueUrl = queueUrl,
                 MaxNumberOfMessages = 1
            });

            if (response == null || response.Messages.Count == 0)
                return;

            var message = response.Messages.FirstOrDefault();
            var result = JsonConvert.DeserializeObject<MemeRequest>(message.Body);

            await processRequest(result);

            await queueClient.DeleteMessageAsync(new DeleteMessageRequest(queueUrl, message.ReceiptHandle));
        }

        public async Task EnqueueMessage(MemeRequest message)
        {
            var messageBody = JsonConvert.SerializeObject(message);
            await queueClient.SendMessageAsync(new SendMessageRequest()
            {
                MessageBody = messageBody,
                QueueUrl = queueUrl
            });
        }

        public async Task Clear()
        {
            await queueClient.PurgeQueueAsync(new PurgeQueueRequest()
            {
                QueueUrl = queueUrl
            });
        }
    }
}
