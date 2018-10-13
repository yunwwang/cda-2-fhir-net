using System.IO;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

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

            bundle.Identifier.Should().NotBeNull();
            bundle.Identifier.Value.Should().NotBeNullOrEmpty();
            bundle.Identifier.System.Should().NotBeNullOrEmpty();

            bundle.Entry[0].Resource.ResourceType.Should().Be(ResourceType.Composition);

            var composition = (Composition) bundle.Entry[0].Resource;

            composition.Meta.Profile.Count().Should().BeGreaterThan(0);
            composition.Type.Should().NotBeNull();
            composition.Type.Coding.Count.Should().Be(1);
            composition.Title.Should().NotBeNullOrEmpty();
            composition.Date.Should().NotBeNullOrEmpty();
            composition.Confidentiality.Should().NotBeNull();
            composition.Language.Should().NotBeNullOrEmpty();

            using (var writer = new StreamWriter("output.json"))
            using (var jWriter = new JsonTextWriter(writer))
            {
                jWriter.Formatting = Formatting.Indented;
                new FhirJsonSerializer().Serialize(bundle, jWriter);
            }
        }
    }
}
