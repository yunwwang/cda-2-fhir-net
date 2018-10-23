using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Extension;
using Wyw.Cda2Fhir.Core.Serialization.DataType;

namespace Wyw.Cda2Fhir.Core.Serialization.Resource
{
    public class PractitionerParser : BaseParser<Practitioner>
    {
        public PractitionerParser()
        {
        }

        public PractitionerParser(Bundle bundle) : base(bundle)
        {
        }

        public override Practitioner FromXml(XElement element)
        {
            if (element == null)
                return null;

            var practitioner = new Practitioner
            {
                Id = Guid.NewGuid().ToString(),
                Meta = new Meta
                {
                    Profile = new List<string>
                    {
                        "http://hl7.org/fhir/us/core/StructureDefinition/us-core-practitioner"
                    }
                }
            };

            var role = new PractitionerRole()
            {
                Id = Guid.NewGuid().ToString(),
                Practitioner = new ResourceReference($"{practitioner.TypeName}/{practitioner.Id}")
            };

            foreach (var child in element.Elements())
                if (child.Name.LocalName == "id")
                {
                    var id = FromXml(new IdentifierParser(), child);

                    if (id != null)
                        practitioner.Identifier.Add(id);
                }
                else if (child.Name.LocalName == "code")
                {
                    var code = FromXml(new CodeableConceptParser(), element);
                    if (code != null)
                        role.Specialty.Add(code);
                }
                else if (child.Name.LocalName == "addr")
                {
                    var addr = FromXml(new AddressParser(), child);
                    if (addr != null)
                        practitioner.Address.Add(addr);
                }
                else if (child.Name.LocalName == "telecom")
                {
                    var telecom = FromXml(new ContactPointParser(), child);
                    if (telecom != null)
                        practitioner.Telecom.Add(telecom);
                }
                else if (child.Name.LocalName == "assignedPerson")
                {
                    var name = FromXml(new HumanNameParser(), child.CdaElement("name"));
                    if (name != null)
                        practitioner.Name.Add(name);
                }
                else if (child.Name.LocalName == "receivedOrganization")
                {
                    // extension?
                }

            var existingPractitioner = Bundle.FirstOrDefault<Practitioner>(p => p.Identifier.Matches(practitioner.Identifier));

            if (existingPractitioner == null)
            {
                Bundle?.Entry.Add(new Bundle.EntryComponent(){Resource = practitioner});
                Bundle?.Entry.Add(new Bundle.EntryComponent(){Resource = role});
            }
            else
            {
                practitioner = existingPractitioner;
            }
            return practitioner;
        }


        private void AddPractitionerRole(Practitioner practitioner, XElement element)
        {
            var code = FromXml(new CodeableConceptParser(), element);

            if (code == null) return;

            var role = new PractitionerRole
            {
                Id = Guid.NewGuid().ToString(),
                Specialty = new List<CodeableConcept>{code},
                Practitioner = new ResourceReference($"{practitioner.TypeName}/{practitioner.Id}")
            };

            Bundle?.Entry.Add(new Bundle.EntryComponent {Resource = role});
        }
    }
}