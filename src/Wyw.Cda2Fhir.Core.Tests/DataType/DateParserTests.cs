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
    public class DateParserTests
    {
        [TestMethod]
        public void NullXmlShallRenturnNull()
        {
            var result = new DateParser().FromXml(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ElementWithoutValueShallRenturnNull()
        {
            var xml = @"<brithTime/>";

            var element = XElement.Parse(xml);
            var result = new DateParser().FromXml(element);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ValueWithoutYearShallReturnNull()
        {
            var xml = @"<brithTime value=""201""/>";

            var element = XElement.Parse(xml);
            var result = new DateParser().FromXml(element);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallReturnYear()
        {
            var xml = @"<brithTime value=""20130815""/>";

            var element = XElement.Parse(xml);
            var result = new DateParser().FromXml(element);
            result.Should().NotBeNull();

            var dateTime = result.ToDateTime();

            dateTime?.Year.Should().Be(2013);
            dateTime?.Month.Should().Be(08);
            dateTime?.Day.Should().Be(15);
        }
    }
}
