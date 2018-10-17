using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Serialization.DataType;

namespace Wyw.Cda2Fhir.Core.Tests.DataType
{
    [TestClass]
    public class HumanNameParserTests
    {
        [TestMethod]
        public void NullXmlShallRenturnNull()
        {
            var result = new HumanNameParser().FromXml(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallRenturnHomeAddress()
        {
            var xml =
                @"<name use=""L"">
                    <given>Eve</given>
                    <family qualifier=""SP"">Betterhalf</family>
                </name>";

            var element = XElement.Parse(xml);
            var result = new HumanNameParser().FromXml(element);
            result.Should().NotBeNull();
            result.Use.Should().Be(HumanName.NameUse.Usual);
            result.Family.Should().Be("Betterhalf");
            result.Given.Count().Should().Be(1);
            result.Given.First().Should().Be("Eve");
        }
    }
}
