using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wyw.Cda2Fhir.Core.Serialization.ValueSet;

namespace Wyw.Cda2Fhir.Core.Tests.ValueSet
{
    [TestClass]
    public class AdministrativeGenderParserTests
    {
        [TestMethod]
        public void NullXmlShallRenturnNull()
        {
            var result = new AdministrativeGenderParser().FromCda(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void NotMappedUseAttributeShallRenturnNull()
        {
            var result = new AdministrativeGenderParser().FromCda("BAD");
            result.Should().BeNull();
        }

        [TestMethod]
        public void ShallReturnMale()
        {
            var result = new AdministrativeGenderParser().FromCda("M");
            result.Should().Be(AdministrativeGender.Male);
        }

        [TestMethod]
        public void ShallReturnFemale()
        {
            var result = new AdministrativeGenderParser().FromCda("F");
            result.Should().Be(AdministrativeGender.Female);
        }

        [TestMethod]
        public void ShallReturnOther()
        {
            var result = new AdministrativeGenderParser().FromCda("UN");
            result.Should().Be(AdministrativeGender.Other);
        }

        [TestMethod]
        public void ShallReturnUnknown()
        {
            var result = new AdministrativeGenderParser().FromCda("UNK");
            result.Should().Be(AdministrativeGender.Unknown);
        }
    }
}
