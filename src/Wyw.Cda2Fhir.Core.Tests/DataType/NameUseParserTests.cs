using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using FluentAssertions;
using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wyw.Cda2Fhir.Core.Serialization.DataType;

namespace Wyw.Cda2Fhir.Core.Tests.DataType
{
    [TestClass]
    public class NameUseParserTests
    {
        [TestMethod]
        public void NullXmlShallRenturnNull()
        {
            var result = new NameUseParser().FromXml(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void NotMappedUseAttributeShallRenturnNull()
        {
            var xml = @"<name use=""BAD""/>";

            var element = XElement.Parse(xml);
            var result = new NameUseParser().FromXml(element.Attribute("use"));
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallRenturnOfficialName()
        {
            var xml = @"<name use=""C""/>";

            var element = XElement.Parse(xml);
            var result = new NameUseParser().FromXml(element.Attribute("use"));
            result.Should().Be(HumanName.NameUse.Official);
        }

        [TestMethod]
        public void ShallRenturnUsualName()
        {
            var xml = @"<name use=""L""/>";

            var element = XElement.Parse(xml);
            var result = new NameUseParser().FromXml(element.Attribute("use"));
            result.Should().Be(HumanName.NameUse.Usual);
        }

        [TestMethod]
        public void ShallRenturnNickname()
        {
            var xml = @"<name use=""A""/>";

            var element = XElement.Parse(xml);
            var result = new NameUseParser().FromXml(element.Attribute("use"));
            result.Should().Be(HumanName.NameUse.Nickname);

            xml = @"<name use=""I""/>";

            element = XElement.Parse(xml);
            result = new NameUseParser().FromXml(element.Attribute("use"));
            result.Should().Be(HumanName.NameUse.Nickname);

            xml = @"<name use=""P""/>";

            element = XElement.Parse(xml);
            result = new NameUseParser().FromXml(element.Attribute("use"));
            result.Should().Be(HumanName.NameUse.Nickname);

            xml = @"<name use=""R""/>";

            element = XElement.Parse(xml);
            result = new NameUseParser().FromXml(element.Attribute("use"));
            result.Should().Be(HumanName.NameUse.Nickname);

        }
    }
}
