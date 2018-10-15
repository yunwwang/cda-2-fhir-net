using System.Xml.Linq;
using FluentAssertions;
using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wyw.Cda2Fhir.Core.Serialization.DataType;

namespace Wyw.Cda2Fhir.Core.Tests.DataType
{
    [TestClass]
    public class ContactPointParserTests
    {
        [TestMethod]
        public void NullXmlShallRenturnNull()
        {
            var result = new ContactPointParser().FromXml(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallRenturnHomeContactPoint()
        {
            var xml =
                @"<telecom value=""tel:+1(555)555-2003"" use=""HP""/>";

            var element = XElement.Parse(xml);
            var result = new ContactPointParser().FromXml(element);
            result.Should().NotBeNull();
            result.Use.Should().Be(ContactPoint.ContactPointUse.Home);
            result.Value.Should().Be("+1(555)555-2003");
        }
    }
}