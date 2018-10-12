using System.Xml.Linq;
using FluentAssertions;
using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Wyw.Cda2Fhir.Core.Tests
{
    [TestClass]
    public class Cda2FhirParserTests
    {
        [TestMethod]
        public void CompositionShallHaveId()
        {
            var xml = XDocument.Load("Resource/C-CDA_R2-1_CCD.xml");
            var bundle = new CdaParser().Convert(xml);

            bundle.Should().NotBeNull();
            bundle.Entry[0].Resource.ResourceType.Should().Be(ResourceType.Composition);

            var composition = (Composition) bundle.Entry[0].Resource;

            composition.Identifier.Should().NotBeNull();
            composition.Identifier.Value.Should().NotBeNullOrEmpty();
            composition.Identifier.System.Should().NotBeNullOrEmpty();

            composition.Type.Should().NotBeNull();
            composition.Type.Coding.Count.Should().Be(1);

        }
    }
}
