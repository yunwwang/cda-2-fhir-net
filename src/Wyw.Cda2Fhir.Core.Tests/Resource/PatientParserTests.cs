using System.Xml.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wyw.Cda2Fhir.Core.Extension;
using Wyw.Cda2Fhir.Core.Serialization;

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
            result.Id.Should().NotBeNullOrEmpty();
            result.Identifier.Count.Should().BeGreaterThan(0);
            result.Address.Count.Should().BeGreaterThan(0);
            result.Telecom.Count.Should().BeGreaterThan(0);
        }
    }
}