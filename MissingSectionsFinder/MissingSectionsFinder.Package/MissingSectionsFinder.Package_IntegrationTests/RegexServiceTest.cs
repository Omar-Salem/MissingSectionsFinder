using Services.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Services.Contracts;
using System.Collections.Generic;

namespace MissingSectionsFinder.Package_IntegrationTests
{
    [TestClass()]
    public class RegexServiceTest
    {
        [TestMethod()]
        public void GetSectionsInLayoutTest()
        {
            IRegexService target = new RegexService();
            string pageContent = Resources.;
            IEnumerable<string> actual = target.GetSectionsInLayout(pageContent);
            Assert.AreEqual(expected, actual);
        }
    }
}