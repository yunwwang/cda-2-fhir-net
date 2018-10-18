using FluentAssertions;
using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wyw.Cda2Fhir.Core.Serialization.ValueSet;

namespace Wyw.Cda2Fhir.Core.Tests.ValueSet
{
    [TestClass]
    public class NameUseParserTests
    {
        [TestMethod]
        public void NullXmlShallRenturnNull()
        {
            var result = new NameUseParser().FromCda(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void NotMappedUseAttributeShallRenturnNull()
        {
            var result = new NameUseParser().FromCda("BAD");
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallRenturnOfficialName()
        {
            var result = new NameUseParser().FromCda("C");
            result.Should().Be(HumanName.NameUse.Official);
        }

        [TestMethod]
        public void ShallRenturnUsualName()
        {
            var result = new NameUseParser().FromCda("L");
            result.Should().Be(HumanName.NameUse.Usual);
        }

        [TestMethod]
        public void ShallRenturnNickname()
        {
            var result = new NameUseParser().FromCda("A");
            result.Should().Be(HumanName.NameUse.Nickname);

            result = new NameUseParser().FromCda("I");
            result.Should().Be(HumanName.NameUse.Nickname);

            result = new NameUseParser().FromCda("P");
            result.Should().Be(HumanName.NameUse.Nickname);

            result = new NameUseParser().FromCda("R");
            result.Should().Be(HumanName.NameUse.Nickname);

        }
    }
}
