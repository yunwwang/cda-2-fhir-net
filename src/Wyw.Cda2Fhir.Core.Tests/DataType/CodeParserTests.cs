using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wyw.Cda2Fhir.Core.Serialization.DataType;

namespace Wyw.Cda2Fhir.Core.Tests.DataType
{
    [TestClass]
    public class CodeParserTests
    {
        [TestMethod]
        public void NullXmlShallRenturnNull()
        {
            var result = new CodeParser().FromXml(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ElementWithoutCodeAttributeShallRenturnNull()
        {
            var element = XElement.Parse("<languageCode/>");
            var result = new CodeParser().FromXml(element);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallRenturnCode()
        {
            var element = XElement.Parse(@"<languageCode code=""en-us""/>");
            var result = new CodeParser().FromXml(element);
            result.Should().NotBeNull();
            result.Value.Should().Be("en-us");
        }

    }
}
