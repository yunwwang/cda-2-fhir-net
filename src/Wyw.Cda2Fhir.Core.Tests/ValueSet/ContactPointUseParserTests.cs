using FluentAssertions;
using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wyw.Cda2Fhir.Core.Serialization.ValueSet;

namespace Wyw.Cda2Fhir.Core.Tests.DataType
{
    [TestClass]
    public class ContactPointUseParserTests
    {
        [TestMethod]
        public void NullXmlShallRenturnNull()
        {
            var result = new ContactPointUseParser().FromCda(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void NotMappedUseAttributeShallRenturnNull()
        {
            var result = new ContactPointUseParser().FromCda("BAD");
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallRenturnHomeAddress()
        {
            var result = new ContactPointUseParser().FromCda("H");
            result.Should().Be(ContactPoint.ContactPointUse.Home);

            result = new ContactPointUseParser().FromCda("HP");
            result.Should().Be(ContactPoint.ContactPointUse.Home);

            result = new ContactPointUseParser().FromCda("HV");
            result.Should().Be(ContactPoint.ContactPointUse.Home);
        }

        [TestMethod]
        public void ShallRenturnWorkAddress()
        {
            var result = new ContactPointUseParser().FromCda("WP");
            result.Should().Be(ContactPoint.ContactPointUse.Work);
        }

        [TestMethod]
        public void ShallRenturnTempAddress()
        {
            var result = new ContactPointUseParser().FromCda("TMP");
            result.Should().Be(ContactPoint.ContactPointUse.Temp);
        }

        [TestMethod]
        public void ShallRenturnOldAddress()
        {
            var result = new ContactPointUseParser().FromCda("OLD");
            result.Should().Be(ContactPoint.ContactPointUse.Old);
        }

        [TestMethod]
        public void ShallRenturnMobileContactPoint()
        {
            var result = new ContactPointUseParser().FromCda("MC");
            result.Should().Be(ContactPoint.ContactPointUse.Mobile);
        }
    }
}