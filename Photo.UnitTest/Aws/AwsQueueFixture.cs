using NUnit.Framework;
using Photo.Aws;
using Photo.Core.Models;
using StatePrinter;
using StatePrinter.ValueConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.UnitTest.Aws
{
    [TestFixture]
    public class AwsQueueFixture
    {
        [Test]
        public async Task Enqueue_DequeuesMessage()
        {
            var queue = new AwsQueue("gcad-storage-demo");
            await queue.Clear();

            var expected = new MemeRequest()
            {
                Id = Guid.NewGuid()
            };

            await queue.EnqueueMessage(expected);

            MemeRequest actual = null;
            await queue.DequeueMessage(m => Task.Run(() => actual = m));

            var printer = CreatePrinter();
            printer.Assert.PrintEquals(printer.PrintObject(expected), actual);
        }

        public static Stateprinter CreatePrinter()
        {
            var printer = new Stateprinter();
            printer.Configuration
                .Test.SetAreEqualsMethod(NUnit.Framework.Assert.AreEqual)
                .Add(new StringConverter(""));

            return printer;
        }
    }
}
