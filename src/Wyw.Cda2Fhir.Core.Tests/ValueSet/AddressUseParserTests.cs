using FluentAssertions;
using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wyw.Cda2Fhir.Core.Serialization.ValueSet;

namespace Wyw.Cda2Fhir.Core.Tests.DataType
{
    [TestClass]
    public class AddressUseParserTests
    {
        [TestMethod]
        public void NullXmlShallRenturnNull()
        {
            var result = new AddressUseParser().FromCda(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void NotMappedUseAttributeShallRenturnNull()
        {
            var result = new AddressUseParser().FromCda("BAD");
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallRenturnHomeAddress()
        {
            var result = new AddressUseParser().FromCda("H");
            result.Should().Be(Address.AddressUse.Home);

            result = new AddressUseParser().FromCda("HP");
            result.Should().Be(Address.AddressUse.Home);

            result = new AddressUseParser().FromCda("HV");
            result.Should().Be(Address.AddressUse.Home);

        }

        [TestMethod]
        public void ShallRenturnWorkAddress()
        {
            var result = new AddressUseParser().FromCda("WP");
            result.Should().Be(Address.AddressUse.Work);

            result = new AddressUseParser().FromCda("DIR");
            result.Should().Be(Address.AddressUse.Work);

            result = new AddressUseParser().FromCda("PUB");
            result.Should().Be(Address.AddressUse.Work);
        }

        [TestMethod]
        public void ShallRenturnTempAddress()
        {
            var result = new AddressUseParser().FromCda("TMP");
            result.Should().Be(Address.AddressUse.Temp);
        }

        [TestMethod]
        public void ShallRenturnOldAddress()
        {
            var result = new AddressUseParser().FromCda("OLD");
            result.Should().Be(Address.AddressUse.Old);
        }

    }
}
