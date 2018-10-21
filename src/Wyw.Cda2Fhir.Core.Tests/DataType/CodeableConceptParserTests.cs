using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using Wyw.Cda2Fhir.Core.Serialization.DataType;

namespace Wyw.Cda2Fhir.Core.Tests.DataType
{
    [TestClass]
    public class CodeableConceptParserTests
    {
        [TestMethod]
        public void NullXmlShallRenturnNull()
        {
            var result = new CodeableConceptParser().FromXml(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallRenturnCodeableConcept()
        {
            var xml =
                @"<code code=""34133 - 9"" displayName=""Summarization of Episode Note"" codeSystem=""2.16.840.1.113883.6.1"" codeSystemName=""LOINC"" />";
            var element = XElement.Parse(xml);
            var result = new CodeableConceptParser().FromXml(element);
            result.Should().NotBeNull();
            result.Coding.Count.Should().BeGreaterThan(0);
        }
    }
}
