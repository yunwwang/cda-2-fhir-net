using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wyw.Cda2Fhir.Core.Extension;
using Wyw.Cda2Fhir.Core.Serialization.Resource;

namespace Wyw.Cda2Fhir.Core.Tests.Resource
{
    [TestClass]
    public class PractitionerParserTests
    {
        [TestMethod]
        public void NullElementShallReturnNull()
        {
            var result = new PractitionerParser().FromXml(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallReturnOrganization()
        {
            var xml = XDocument.Load("C-CDA_R2-1_CCD.xml");
            var element = xml.Root.CdaElement("author");

            var result = new PractitionerParser().FromXml(element);
            result.Should().NotBeNull();
            // Shall have id
            result.Id.Should().NotBeNullOrEmpty();
            // US-Core Shall have identifier
            result.Identifier.Count.Should().BeGreaterThan(0);
            result.Identifier.All(i => !string.IsNullOrEmpty(i.System)).Should().BeTrue();
            // US-Core Shall have name
            result.Name.Count.Should().Be(1);
            result.Name.All(n => !string.IsNullOrEmpty(n.Family)).Should().BeTrue();
        }
    }
}
