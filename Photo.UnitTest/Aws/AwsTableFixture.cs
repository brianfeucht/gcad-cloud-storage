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
    public class AwsTableFixture
    {
        [Test]
        public async Task AddGetFind_GoldenPath()
        {
            var table = new AwsTable("gcad-demo");

            var expected = new CompletedMeme()
            {
                CreatedOn = DateTime.UtcNow,
                Text = "UnitTest",
                ImageUrl = "http://test.test/test.png"
            };

            var id = await table.Add(expected);

            expected.Id = id;

            var actual = await table.Get(id);

            var printer = CreatePrinter();
            printer.Assert.PrintEquals(printer.PrintObject(expected), actual);

            var collection = await table.Latest();

            Assert.That(collection.Any(m => m.Id == id));
        }

        //TODO: Refactor to Utilities class post merge.
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
