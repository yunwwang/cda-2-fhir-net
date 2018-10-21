using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wyw.Cda2Fhir.Core.Extension;
using Wyw.Cda2Fhir.Core.Serialization;
using Wyw.Cda2Fhir.Core.Serialization.Rersource;

namespace Wyw.Cda2Fhir.Core.Tests.Resource
{
    [TestClass]
    public class PatientParserTests
    {
        [TestMethod]
        public void NullElementShallReturnNull()
        {
            var result = new PatientParser().FromXml(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallReturnPatient()
        {
            var xml = XDocument.Load("C-CDA_R2-1_CCD.xml");
            var element = xml.Root.CdaElement("recordTarget")?.CdaElement("patientRole");

            var result = new PatientParser().FromXml(element);
            result.Should().NotBeNull();
            // Shall have id
            result.Id.Should().NotBeNullOrEmpty();
            // US-Core Shall have identifier
            result.Identifier.Count.Should().BeGreaterThan(0);
            result.Identifier.All(i => !string.IsNullOrEmpty(i.System) && !string.IsNullOrEmpty(i.Value)).Should()
                .BeTrue();
            // US-Core Shall have name
            result.Name.Count.Should().BeGreaterThan(0);
            result.Name.All(n => !string.IsNullOrEmpty(n.Family) && n.Given.Any()).Should().BeTrue();
            // US-Core Shall have gender
            result.Gender.Should().NotBeNull();

            result.Address.Count.Should().BeGreaterThan(0);
            result.Telecom.Count.Should().BeGreaterThan(0);
            result.BirthDate.Should().NotBeNullOrEmpty();
            result.Extension.Count(e => e.Url == "http://hl7.org/fhir/us/core/StructureDefinition/us-core-race").Should()
                .Be(1);
            result.Extension.Count(e => e.Url == "http://hl7.org/fhir/us/core/StructureDefinition/us-core-ethnicity")
                .Should().Be(1);
        }
    }
}