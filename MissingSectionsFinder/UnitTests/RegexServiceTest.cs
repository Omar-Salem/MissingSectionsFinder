using Services.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Services.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    [TestClass()]
    public class RegexServiceTest
    {
        [TestMethod()]
        public void GetSectionsInLayoutTest()
        {
            //Arrange
            IRegexService target = new RegexService();
            string pageContent = Resources.Layout;

            //Act
            IEnumerable<string> actual = target.GetSectionsInLayout(pageContent);

            //Assert
            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual("Title", actual.ElementAt(0));
            Assert.AreEqual("PageName", actual.ElementAt(1));
        }

        [TestMethod()]
        public void GetLayoutPageName_HardCodedLayout()
        {
            //Arrange
            IRegexService target = new RegexService();
            string pageContent = Resources.PageWithHardCodedLayout;
            string expected = "_Main.cshtml";

            //Act
            string actual = target.GetLayoutPageName(pageContent);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetLayoutPageName_MVCLayout()
        {
            //Arrange
            IRegexService target = new RegexService();
            string pageContent = Resources.PageWithMVCLayout;
            string expected = "MVC.Shared.Views.ViewNames._Filters";

            //Act
            string actual = target.GetLayoutPageName(pageContent);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetSectionsInPageTest()
        {
            //Arrange
            IRegexService target = new RegexService();
            string pageContent = Resources.PageWithSections;

            //Act
            IEnumerable<string> actual = target.GetSectionsInPage(pageContent);

            //Assert
            Assert.AreEqual(3, actual.Count());
            Assert.AreEqual("Filters", actual.ElementAt(0));
            Assert.AreEqual("Title", actual.ElementAt(1));
            Assert.AreEqual("PageName", actual.ElementAt(2));
        }
    }
}