using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Serialization.DataType;

namespace Wyw.Cda2Fhir.Core.Tests.DataType
{
    [TestClass]
    public class FhirDateTimeParserTests
    {
        [TestMethod]
        public void NullXmlShallRenturnNull()
        {
            var result = new FhirDateTimeParser().FromXml(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ElementWithoutValueShallRenturnNull()
        {
            var xml = @"<effectiveTime/>";

            var element = XElement.Parse(xml);
            var result = new FhirDateTimeParser().FromXml(element);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ValueWithoutYearShallReturnNull()
        {
            var xml = @"<effectiveTime value=""201""/>";

            var element = XElement.Parse(xml);
            var result = new FhirDateTimeParser().FromXml(element);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallReturnYear()
        {
            var xml = @"<effectiveTime value=""201308151030-0800""/>";

            var element = XElement.Parse(xml);
            var result = new FhirDateTimeParser().FromXml(element);
            result.Should().NotBeNull();

            var dateTime = result.ToDateTime();

            dateTime?.Kind.Should().Be(DateTimeKind.Utc);
            dateTime?.Year.Should().Be(2013);
            dateTime?.Month.Should().Be(08);
            dateTime?.Day.Should().Be(15);
            dateTime?.Hour.Should().Be(18);
            dateTime?.Minute.Should().Be(30);
        }
    }
}
