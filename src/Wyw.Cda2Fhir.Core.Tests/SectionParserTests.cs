using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wyw.Cda2Fhir.Core.Extension;
using Wyw.Cda2Fhir.Core.Serialization;
using Wyw.Cda2Fhir.Core.Serialization.Resource;

namespace Wyw.Cda2Fhir.Core.Tests
{
    [TestClass]
    public class SectionParserTests
    {
        [TestMethod]
        public void NullElementShallReturnNull()
        {
            var result = new SectionParser().FromXml(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallReturnSectionComponent()
        {
            var xml = XDocument.Load("C-CDA_R2-1_CCD.xml");
            var element = xml.Root.CdaElement("component").CdaElement("structuredBody").CdaElement("component").CdaElement("section");

            var result = new SectionParser().FromXml(element);
            result.Should().NotBeNull();
            // US-Core Shall have title
            result.Title.Should().NotBeNullOrEmpty();
            // US-Core Shall have code
            result.Code?.Coding.Any().Should().BeTrue();
            // Us-Core Shall have text
            result.Text?.Div.Should().NotBeNullOrEmpty();
        }
    }
}