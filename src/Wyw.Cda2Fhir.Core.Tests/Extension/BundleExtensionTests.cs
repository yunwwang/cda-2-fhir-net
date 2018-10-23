using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wyw.Cda2Fhir.Core.Extension;

namespace Wyw.Cda2Fhir.Core.Tests.Extension
{
    [TestClass]
    public class BundleExtensionTests
    {
        [TestMethod]
        public void ShallFindPractitionerWithSameIdentifier()
        {
            var bundle = new Bundle
            {
                Entry = new List<Bundle.EntryComponent>()
                {
                    new Bundle.EntryComponent()
                    {
                        Resource = new Practitioner()
                        {
                            Identifier = new List<Identifier>()
                            {
                                new Identifier("system1", "value1"),
                                new Identifier("system2", "value2")
                            }
                        }
                    },
                    new Bundle.EntryComponent()
                    {
                        Resource = new Practitioner()
                        {
                            Identifier = new List<Identifier>()
                            {
                                new Identifier("system3", "value3")
                            }
                        }
                    },

                }
            };

            var id = new Identifier("system1", "value1");

            var result = bundle.FirstOrDefault<Practitioner>(p => p.Identifier.Any(i => i.Matches(id)));

            result.Should().NotBeNull();
        }
    }
}
