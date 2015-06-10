using NUnit.Framework;
using Photo.Core;
using Photo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.UnitTest.Core
{
    [TestFixture]
    public class LocalQueueFixture
    {
        [Test]
        public async Task Enqueue_EnqueuesMessage()
        {
            var localQueue = new LocalQueue();
            await localQueue.EnqueueMessage(new MemeRequest());

            Assert.AreEqual(1, localQueue.queue.Count);
        }

        [Test]
        public async Task Enqueue_NotifiesOfNewMessage()
        {
            var wasNotified = false;
            var localQueue = new LocalQueue();
            localQueue.NewMessageArrived += (s,e) => { wasNotified = true; };

            await localQueue.EnqueueMessage(new MemeRequest());

            Assert.IsTrue(wasNotified);
        }

        [Test]
        public async Task Enqueue_DequeuesMessage()
        {
            var localQueue = new LocalQueue();
            var expected = new MemeRequest();
            await localQueue.EnqueueMessage(expected);
            await localQueue.EnqueueMessage(new MemeRequest());

            var actual = await localQueue.DequeueMessage();

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(1, localQueue.queue.Count);
        }
    }
}
