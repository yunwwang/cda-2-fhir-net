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
    public class OrganizationParserTests
    {
        [TestMethod]
        public void NullElementShallReturnNull()
        {
            var result = new OrganizationParser().FromXml(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallReturnOrganization()
        {
            var xml = XDocument.Load("C-CDA_R2-1_CCD.xml");
            var element = xml.Root.CdaElement("recordTarget")?.CdaElement("patientRole")?.CdaElement("providerOrganization");

            var result = new OrganizationParser().FromXml(element);
            result.Should().NotBeNull();
            // Shall have id
            result.Id.Should().NotBeNullOrEmpty();
            // US-Core Shall have identifier
            result.Identifier.Count.Should().BeGreaterThan(0);
            // US-Core Shall have name
            result.Name.Should().NotBeNullOrEmpty();
            // US-Core Shall have telcom
            result.Telecom.Count.Should().BeGreaterThan(0);
            // US-Core Shall have address
            result.Address.Count.Should().BeGreaterThan(0);
        }
    }
}