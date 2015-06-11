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
        private readonly AzureQueue queue;

        public AzureQueueFixture()
        {
            queue = new AzureQueue("unittest");
        }

        [Test]
        public async Task Enqueue_DequeuesMessage()
        {
            await queue.EnsureQueueHasBeenCreated();
            await queue.Clear();

            var expected = new MemeRequest()
            {
                Id = Guid.NewGuid()
            };

            await queue.EnqueueMessage(expected);

            MemeRequest actual = null;
            await queue.DequeueMessage(m => Task.Run(() => actual = m));

            var printer = Utilities.CreatePrinter();
            printer.Assert.PrintEquals(printer.PrintObject(expected), actual);
        }
    }
}
