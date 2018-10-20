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
    public class RelatedPersonParserTests
    {
        [TestMethod]
        public void NullElementShallReturnNull()
        {
            var result = new RelatedPersonParser().FromXml(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallReturnPatient()
        {
            var xml = XDocument.Load("C-CDA_R2-1_CCD.xml");
            var element = xml.Root.CdaElement("recordTarget")?.CdaElement("patientRole")?.CdaElement("patient")?.CdaElement("guardian");

            var result = new RelatedPersonParser().FromXml(element);
            result.Should().NotBeNull();
            // Shall have id
            result.Id.Should().NotBeNullOrEmpty();
            result.Name.Count.Should().BeGreaterThan(0);
            result.Name.All(n => !string.IsNullOrEmpty(n.Family) && n.Given.Any()).Should().BeTrue();
        }
    }
}