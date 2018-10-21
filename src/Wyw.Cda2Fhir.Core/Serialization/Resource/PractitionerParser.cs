using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Wyw.Cda2Fhir.Core.Serialization.DataType;
using Wyw.Cda2Fhir.Core.Extension;

namespace Wyw.Cda2Fhir.Core.Serialization.Resource
{
    public class PractitionerParser : BaseParser<Practitioner>
    {
        public PractitionerParser() { }

        public PractitionerParser(Bundle bundle)
        {
            Bundle = bundle;
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

            foreach(var child in element.Elements())
            {
                if (child.Name.LocalName == "id")
                {
                    var id = FromXml(new IdentifierParser(), child);

                    if (id != null)
                        practitioner.Identifier.Add(id);
                }
                else if (child.Name.LocalName == "code")
                {
                    var role = FromXml(new PratitionerRoleParser(), child);
                    if (role != null)
                    {
                        role.Practitioner = new ResourceReference($"{practitioner.TypeName}/{practitioner.Id}");
                        Bundle.Entry.Add(new Bundle.EntryComponent { Resource = role });
                    }
                }
                else if (child.Name.LocalName == "addr")
                {
                    var addr = FromXml(new AddressParser(), child);
                    if(addr != null)
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
            }

            return practitioner;
        }
    }
}
