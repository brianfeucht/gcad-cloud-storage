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
    public class AzureTableFixture
    {
        [Test]
        public async Task AddGetFind_GoldenPath()
        {
            var localTable = new AzureTable("unittest");
            await localTable.EnsureTableHasBeenCreated();

            var expected = new CompletedMeme()
            {
                CreatedOn = DateTime.UtcNow,
                Text = "UnitTest",
                ImageUrl = "http://test.test/test.png"
            };

            var id = await localTable.Add(expected);

            expected.Id = id;

            var actual = await localTable.Get(id);

            var printer = Utilities.CreatePrinter();
            printer.Assert.PrintEquals(printer.PrintObject(expected), actual);

            var collection = await localTable.Latest();

            Assert.That(collection.Any(m => m.Id == id));
        }
    }
}
