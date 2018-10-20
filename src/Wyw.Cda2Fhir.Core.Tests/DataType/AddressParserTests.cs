using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Serialization.DataType;

namespace Wyw.Cda2Fhir.Core.Tests.DataType
{
    [TestClass]
    public class AddressParserTests
    {
        [TestMethod]
        public void NullXmlShallRenturnNull()
        {
            var result = new AddressParser().FromXml(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallRenturnHomeAddress()
        {
            var xml =
                @"<addr use=""H"">
                    <streetAddressLine>2222 Home Street</streetAddressLine>
                    <city>Beaverton</city>
                    <state>OR</state>
                    <postalCode>97867</postalCode>
                    <country>US</country>
                </addr>";

            var element = XElement.Parse(xml);
            var result = new AddressParser().FromXml(element);
            result.Resource.Should().NotBeNull();
            var addr = (Address) result.Resource;

            addr.Use.Should().Be(Address.AddressUse.Home);
            addr.Line.First().Should().Be("2222 Home Street");
            addr.City.Should().Be("Beaverton");
            addr.State.Should().Be("OR");
            addr.PostalCode.Should().Be("97867");
            addr.Country.Should().Be("US");
        }
    }
}
