using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using Wyw.Cda2Fhir.Core.Serialization.DataType;

namespace Wyw.Cda2Fhir.Core.Tests.DataType
{
    [TestClass]
    public class CodingParserTests
    {
        [TestMethod]
        public void NullXmlShallRenturnNull()
        {
            var result = new CodingParser().FromXml(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ElementWithoutCodeSystemAttributeShallRenturnNull()
        {
            var xml =
                @"<code code=""34133-9"" displayName=""Summarization of Episode Note"" codeSystemName=""LOINC"" />";

            var element = XElement.Parse(xml);
            var result = new CodingParser().FromXml(element);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ElementWithoutCodeAttributeShallRenturnNull()
        {
            var xml =
                @"<code displayName=""Summarization of Episode Note"" codeSystem=""2.16.840.1.113883.6.1"" codeSystemName=""LOINC"" />";

            var element = XElement.Parse(xml);
            var result = new CodingParser().FromXml(element);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallRenturnNullFlavor()
        {
            var xml =
                @"<code nullFlavor=""NI""/>";

            var element = XElement.Parse(xml);
            var result = new CodingParser().FromXml(element);
            result.Should().NotBeNull();
            result.Code.Should().Be("NI");
            result.System.Should().Be("http://hl7.org/fhir/v3/NullFlavor");
        }

        [TestMethod]
        public void ShallRenturnCoding()
        {
            var xml =
                @"<code code=""34133 - 9"" displayName=""Summarization of Episode Note"" codeSystem=""2.16.840.1.113883.6.1"" codeSystemName=""LOINC"" />";
            var element = XElement.Parse(xml);
            var result = new CodingParser().FromXml(element);
            result.Should().NotBeNull();
            result.Code.Should().Be("34133 - 9");
            result.System.Should().Be("http://loinc.org");
            result.Display.Should().Be("Summarization of Episode Note");
        }
    }
}
