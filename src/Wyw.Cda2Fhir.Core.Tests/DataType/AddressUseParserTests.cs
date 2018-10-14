using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Serialization.DataType;

namespace Wyw.Cda2Fhir.Core.Tests.DataType
{
    [TestClass]
    public class AddressUseParserTests
    {
        [TestMethod]
        public void NullXmlShallRenturnNull()
        {
            var result = new AddressUseParser().FromXml(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void NotMappedUseAttributeShallRenturnNull()
        {
            var xml = @"<addr use=""BAD""/>";

            var element = XElement.Parse(xml);
            var result = new AddressUseParser().FromXml(element.Attribute("use"));
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallRenturnHomeAddress()
        {
            var xml = @"<addr use=""H""/>";

            var element = XElement.Parse(xml);
            var result = new AddressUseParser().FromXml(element.Attribute("use"));
            result.Should().Be(Address.AddressUse.Home);

            xml = @"<addr use=""HP""/>";

            element = XElement.Parse(xml);
            result = new AddressUseParser().FromXml(element.Attribute("use"));
            result.Should().Be(Address.AddressUse.Home);

            xml = @"<addr use=""HV""/>";

            element = XElement.Parse(xml);
            result = new AddressUseParser().FromXml(element.Attribute("use"));
            result.Should().Be(Address.AddressUse.Home);

        }

        [TestMethod]
        public void ShallRenturnWorkAddress()
        {
            var xml = @"<addr use=""WP""/>";

            var element = XElement.Parse(xml);
            var result = new AddressUseParser().FromXml(element.Attribute("use"));
            result.Should().NotBeNull();
            result.Should().Be(Address.AddressUse.Work);

            xml = @"<addr use=""DIR""/>";

            element = XElement.Parse(xml);
            result = new AddressUseParser().FromXml(element.Attribute("use"));
            result.Should().Be(Address.AddressUse.Work);

            xml = @"<addr use=""PUB""/>";

            element = XElement.Parse(xml);
            result = new AddressUseParser().FromXml(element.Attribute("use"));
            result.Should().Be(Address.AddressUse.Work);
        }

        [TestMethod]
        public void ShallRenturnTempAddress()
        {
            var xml = @"<addr use=""TMP""/>";

            var element = XElement.Parse(xml);
            var result = new AddressUseParser().FromXml(element.Attribute("use"));
            result.Should().NotBeNull();
            result.Should().Be(Address.AddressUse.Temp);
        }

        [TestMethod]
        public void ShallRenturnOldAddress()
        {
            var xml = @"<addr use=""OLD""/>";

            var element = XElement.Parse(xml);
            var result = new AddressUseParser().FromXml(element.Attribute("use"));
            result.Should().NotBeNull();
            result.Should().Be(Address.AddressUse.Old);
        }

    }
}
