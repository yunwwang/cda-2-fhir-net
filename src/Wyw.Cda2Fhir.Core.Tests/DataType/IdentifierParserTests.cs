using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using Wyw.Cda2Fhir.Core.Serialization.DataType;

namespace Wyw.Cda2Fhir.Core.Tests.DataType
{
    [TestClass]
    public class IdentifierParserTests
    {
        [TestMethod]
        public void NullXmlShallRenturnNull()
        {
            var result = new IdentifierParser().FromXml(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ElementWithoutCodeSystemAttributeShallRenturnNull()
        {
            var xml =
                @"<id extension=""TT988""/>";

            var element = XElement.Parse(xml);
            var result = new IdentifierParser().FromXml(element);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallRenturnIdentifier()
        {
            var xml =
                @"<id extension=""TT988"" root=""2.16.840.1.113883.19.5.99999.1""/>";
            var element = XElement.Parse(xml);
            var result = new IdentifierParser().FromXml(element);
            result.Should().NotBeNull();
            result.Value.Should().Be("TT988");
            result.System.Should().Be("urn:oid:2.16.840.1.113883.19.5.99999.1");
        }
    }
}
