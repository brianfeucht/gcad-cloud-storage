using NUnit.Framework;
using Photo.Azure;
using Photo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.UnitTest.Azure
{
    [TestFixture]
    public class AzureQueueFixture
    {
        [Test]
        public async Task Enqueue_EnqueuesMessage()
        {
            var queue = new AzureQueue();
            await queue.EnqueueMessage(new MemeRequest());

            Assert.AreEqual(1, queue.Count);
        }

        [Test]
        public async Task Enqueue_NotifiesOfNewMessage()
        {
            var wasNotified = false;
            var queue = new AzureQueue();
            queue.NewMessageArrived += (s, e) => { wasNotified = true; };

            await queue.EnqueueMessage(new MemeRequest());

            Assert.IsTrue(wasNotified);
        }

        [Test]
        public async Task Enqueue_DequeuesMessage()
        {
            var localQueue = new AzureQueue();
            var expected = new MemeRequest();
            await localQueue.EnqueueMessage(expected);
            await localQueue.EnqueueMessage(new MemeRequest());

            var actual = await localQueue.DequeueMessage();

            Assert.AreEqual(expected, actual);
        }
    }
}
