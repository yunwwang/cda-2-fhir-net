using System.IO;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Wyw.Cda2Fhir.Core.Model;

namespace Wyw.Cda2Fhir.Core.Tests
{
    [TestClass]
    public class Cda2FhirParserTests
    {
        [TestMethod]
        public void ShallReturnComposition()
        {
            var xml = XDocument.Load("C-CDA_R2-1_CCD.xml");

            var parserSettings = new CdaParserSettings
            {
                RunValidation = false
            };

            var parser = new CdaParser(parserSettings);
            var bundle = parser.Convert(xml);

            bundle.Should().NotBeNull();
            bundle.Id.Should().NotBeNullOrEmpty();
            bundle.Identifier.Should().NotBeNull();
            bundle.Entry[0].Resource.ResourceType.Should().Be(ResourceType.Composition);

            var composition = (Composition) bundle.Entry[0].Resource;

            composition.Id.Should().NotBeNullOrEmpty();
            composition.Language.Should().NotBeNullOrEmpty();
            composition.Identifier.Should().NotBeNull();
            composition.Subject.Should().NotBeNull();
            composition.Date.Should().NotBeNullOrEmpty();
            composition.Author.Count.Should().BeGreaterThan(0);
            composition.Title.Should().NotBeNullOrEmpty();
            composition.Confidentiality.Should().NotBeNull();
            //composition.Custodian.Should().NotBeNull();
            composition.Type.Should().NotBeNull();


            using (var writer = new StreamWriter("output.json"))
            using (var jWriter = new JsonTextWriter(writer))
            {
                jWriter.Formatting = Formatting.Indented;
                new FhirJsonSerializer().Serialize(bundle, jWriter);
            }
        }

        [TestMethod]
        public void ShallReturnNoValidationError()
        {
            var xml = XDocument.Load("C-CDA_R2-1_CCD.xml", LoadOptions.SetLineInfo);

            var parserSettings = new CdaParserSettings
            {
                RunValidation = true
            };

            var parser = new CdaParser(parserSettings);
            var bundle = parser.Convert(xml);

            using (var writer = new StreamWriter("error.json"))
            using (var jWriter = new JsonTextWriter(writer))
            {
                jWriter.Formatting = Formatting.Indented;
                var serializer = new JsonSerializer();
                serializer.Serialize(jWriter, parser.Errors);
            }

            parser.Errors.Count.Should().Be(0);
        }
    }
}
