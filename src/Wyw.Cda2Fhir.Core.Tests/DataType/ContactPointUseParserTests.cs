using System.Xml.Linq;
using FluentAssertions;
using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wyw.Cda2Fhir.Core.Serialization.DataType;

namespace Wyw.Cda2Fhir.Core.Tests.DataType
{
    [TestClass]
    public class ContactPointUseParserTests
    {
        [TestMethod]
        public void NullXmlShallRenturnNull()
        {
            var result = new ContactPointUseParser().FromXml(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void NotMappedUseAttributeShallRenturnNull()
        {
            var xml = @"<addr use=""BAD""/>";

            var element = XElement.Parse(xml);
            var result = new ContactPointUseParser().FromXml(element.Attribute("use"));
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallRenturnHomeAddress()
        {
            var xml = @"<addr use=""H""/>";

            var element = XElement.Parse(xml);
            var result = new ContactPointUseParser().FromXml(element.Attribute("use"));
            result.Should().Be(ContactPoint.ContactPointUse.Home);

            xml = @"<addr use=""HP""/>";

            element = XElement.Parse(xml);
            result = new ContactPointUseParser().FromXml(element.Attribute("use"));
            result.Should().Be(ContactPoint.ContactPointUse.Home);

            xml = @"<addr use=""HV""/>";

            element = XElement.Parse(xml);
            result = new ContactPointUseParser().FromXml(element.Attribute("use"));
            result.Should().Be(ContactPoint.ContactPointUse.Home);
        }

        [TestMethod]
        public void ShallRenturnWorkAddress()
        {
            var xml = @"<addr use=""WP""/>";

            var element = XElement.Parse(xml);
            var result = new ContactPointUseParser().FromXml(element.Attribute("use"));
            result.Should().NotBeNull();
            result.Should().Be(ContactPoint.ContactPointUse.Work);
        }

        [TestMethod]
        public void ShallRenturnTempAddress()
        {
            var xml = @"<addr use=""TMP""/>";

            var element = XElement.Parse(xml);
            var result = new ContactPointUseParser().FromXml(element.Attribute("use"));
            result.Should().NotBeNull();
            result.Should().Be(ContactPoint.ContactPointUse.Temp);
        }

        [TestMethod]
        public void ShallRenturnOldAddress()
        {
            var xml = @"<addr use=""OLD""/>";

            var element = XElement.Parse(xml);
            var result = new ContactPointUseParser().FromXml(element.Attribute("use"));
            result.Should().NotBeNull();
            result.Should().Be(ContactPoint.ContactPointUse.Old);
        }

        [TestMethod]
        public void ShallRenturnMobileContactPoint()
        {
            var xml = @"<addr use=""MC""/>";

            var element = XElement.Parse(xml);
            var result = new ContactPointUseParser().FromXml(element.Attribute("use"));
            result.Should().NotBeNull();
            result.Should().Be(ContactPoint.ContactPointUse.Mobile);
        }
    }
}