using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wyw.Cda2Fhir.Core.Extension;
using Wyw.Cda2Fhir.Core.Serialization;
using Wyw.Cda2Fhir.Core.Serialization.Resource;

namespace Wyw.Cda2Fhir.Core.Tests.Resource
{
    [TestClass]
    public class AllergyIntoleranceParserTests
    {
        [TestMethod]
        public void NullElementShallReturnNull()
        {
            var result = new AllergyIntoleranceParser().FromXml(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallReturnOrganization()
        {
            var xml = XDocument.Load("C-CDA_R2-1_CCD.xml");

            foreach (var comp in xml.Root.CdaElement("component").CdaElement("structuredBody").CdaElements("component"))
            {
                var section = comp.CdaElement("section");

                if (section.CdaElement("code").Attribute("code")?.Value == "48765 - 2")
                {
                    var element = section.CdaElement("entry").CdaElement("act");

                    var result = new AllergyIntoleranceParser().FromXml(element);
                    result.Should().NotBeNull();
                    // Shall have id
                    result.Id.Should().NotBeNullOrEmpty();
                    // Shall have verificationStatus
                    result.VerificationStatus.Should().NotBeNull();
                    // Shall have code
                    // US-Core Shall have patient
                    break;
                }
            }
        }
    }
}