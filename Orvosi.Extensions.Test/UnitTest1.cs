using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Globalization;
using WebApp.Library.Extensions;

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
        
        [TestMethod]
        public void DateTimeFormats()
        {
            var now = new DateTime(2015, 01, 13, 12, 59, 59);
            var formatted = now.GetDateTimeFormats('d')[5];

        }

        [TestMethod]
        public void RestOfWeek()
        {
            var t = DateTime.Now.GetRestOfWeek();
            var e = t;
        }
    }
}
