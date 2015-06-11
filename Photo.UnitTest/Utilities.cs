using StatePrinter;
using StatePrinter.ValueConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.UnitTest
{
    public class Utilities
    {
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
