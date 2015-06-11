using NUnit.Framework;
using Photo.Core;
using Photo.Core.Models;
using StatePrinter;
using StatePrinter.ValueConverters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.UnitTest.Core
{
    [TestFixture]
    public class LocalTableFixture
    {
        [Test]
        public async Task AddGetFind_GoldenPath()
        {
            var localTable = new LocalTable(Path.GetTempPath());

            var expected = new CompletedMeme()
            {
                CreatedOn = DateTime.UtcNow,
                Text = "UnitTest",
                ImageUrl = "http://test.test/test.png"
            };

            var id = await localTable.Add(expected);

            expected.Id = id;

            var actual = await localTable.Get(id);

            var printer = CreatePrinter();
            printer.Assert.PrintEquals(printer.PrintObject(expected), actual);

            var collection = await localTable.Latest();

            Assert.That(collection.Any(m => m.Id == id));
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
