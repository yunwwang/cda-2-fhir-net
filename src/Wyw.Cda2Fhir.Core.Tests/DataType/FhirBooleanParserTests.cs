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
    public class FhirBooleanParserTests
    {
        [TestMethod]
        public void NullXmlShallReturnNull()
        {
            var result = new FhirBooleanParser().FromXml(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void NonBooleanShallReturnNull()
        {
            var element = XElement.Parse(@"<language value=""1""/>");
            var result = new FhirBooleanParser().FromXml(element);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallReturnTrue()
        {
            var element = XElement.Parse(@"<language value=""true""/>");
            var result = new FhirBooleanParser().FromXml(element);

            result.Should().NotBeNull();
            result.Value.HasValue.Should().BeTrue();
            result.Value.GetValueOrDefault().Should().Be(true);
        }

        [TestMethod]
        public void ShallReturnFalse()
        {
            var element = XElement.Parse(@"<language value=""false""/>");
            var result = new FhirBooleanParser().FromXml(element);

            result.Should().NotBeNull();
            result.Value.HasValue.Should().BeTrue();
            result.Value.GetValueOrDefault().Should().Be(false);
        }
    }
}
