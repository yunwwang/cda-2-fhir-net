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
        public void ElementWithoutUseAttributeShallRenturnNull()
        {
            var xml =
                @"<addr>
                    <streetAddressLine>2222 Home Street</streetAddressLine>
                    <city>Beaverton</city>
                    <state>OR</state>
                    <postalCode>97867</postalCode>
                    <country>US</country>
                </addr>";

            var element = XElement.Parse(xml);
            var result = new AddressParser().FromXml(element);
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
            result.Should().NotBeNull();
            result.Use.Should().Be(Address.AddressUse.Home);
            result.Line.First().Should().Be("2222 Home Street");
            result.City.Should().Be("Beaverton");
            result.State.Should().Be("OR");
            result.PostalCode.Should().Be("97867");
            result.Country.Should().Be("US");

            xml =
                @"<addr use=""HP"">
                </addr>";

            element = XElement.Parse(xml);
            result = new AddressParser().FromXml(element);
            result.Use.Should().Be(Address.AddressUse.Home);

            xml =
                @"<addr use=""HV"">
                </addr>";

            element = XElement.Parse(xml);
            result = new AddressParser().FromXml(element);
            result.Use.Should().Be(Address.AddressUse.Home);

        }

        [TestMethod]
        public void ShallRenturnWorkAddress()
        {
            var xml =
                @"<addr use=""WP"">
                </addr>";

            var element = XElement.Parse(xml);
            var result = new AddressParser().FromXml(element);
            result.Should().NotBeNull();
            result.Use.Should().Be(Address.AddressUse.Work);

            xml =
                @"<addr use=""DIR"">
                </addr>";

            element = XElement.Parse(xml);
            result = new AddressParser().FromXml(element);
            result.Use.Should().Be(Address.AddressUse.Work);

            xml =
                @"<addr use=""PUB"">
                </addr>";

            element = XElement.Parse(xml);
            result = new AddressParser().FromXml(element);
            result.Use.Should().Be(Address.AddressUse.Work);
        }

        [TestMethod]
        public void ShallRenturnTempAddress()
        {
            var xml =
                @"<addr use=""TMP"">
                </addr>";

            var element = XElement.Parse(xml);
            var result = new AddressParser().FromXml(element);
            result.Should().NotBeNull();
            result.Use.Should().Be(Address.AddressUse.Temp);
        }

        [TestMethod]
        public void ShallRenturnOldAddress()
        {
            var xml =
                @"<addr use=""OLD"">
                </addr>";

            var element = XElement.Parse(xml);
            var result = new AddressParser().FromXml(element);
            result.Should().NotBeNull();
            result.Use.Should().Be(Address.AddressUse.Old);
        }

    }
}
