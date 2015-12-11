using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Orvosi.Extensions.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var prebuilder = new StringBuilder();
            prebuilder.Append("test line 1");
            prebuilder.AppendLine();
            prebuilder.Append("test line 2");
            prebuilder.AppendLine();

            var postbuilder = new StringBuilder();
            postbuilder.Append("test line 1");
            postbuilder.AppendLine();

            prebuilder.RemoveLine("test line 2");

            Assert.AreEqual(prebuilder.ToString(), postbuilder.ToString());
        }
    }
}
