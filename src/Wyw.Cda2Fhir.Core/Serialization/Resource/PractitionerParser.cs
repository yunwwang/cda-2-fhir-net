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

            var location = new Location
            {
                Id = Guid.NewGuid().ToString()
            };

            foreach (var child in element.Elements())
                switch (child.Name.LocalName)
                {
                    case "id":
                        var id = FromXml(new IdentifierParser(), child);
                        if (id != null)
                            practitioner.Identifier.Add(id);
                        break;
                    case "code":
                        var code = FromXml(new CodeableConceptParser(), child);
                        if (code != null)
                            role.Specialty.Add(code);
                        break;
                    case "addr":
                        location.Address = FromXml(new AddressParser(), child);
                        break;
                    case "telecom":
                        var telecom = FromXml(new ContactPointParser(), child);
                        if (telecom != null)
                            location.Telecom.Add(telecom);
                        break;
                    case "assignedPerson":
                    case "informationRecipient":
                        var name = FromXml(new HumanNameParser(), child.CdaElement("name"));
                        if (name != null)
                            practitioner.Name.Add(name);
                        break;
                    case "receivedOrganization":
                        break;
                }

            var existingPractitioner = Bundle?.FirstOrDefault<Practitioner>(p => p.Identifier.Matches(practitioner.Identifier));

            if (existingPractitioner != null)
            {
                practitioner = existingPractitioner;
            }
            else
            {
                Bundle?.AddResourceEntry(practitioner, null);
            }

            role.Practitioner = practitioner.GetResourceReference();

            if (location.Address != null || location.Telecom.Any())
            {
                var existingLocation = Bundle?.FirstOrDefault<Location>(l =>
                    l.Address.Matches(location.Address) && l.Telecom.Matches(location.Telecom));

                if (existingLocation != null)
                    location = existingLocation;
                else
                    Bundle?.AddResourceEntry(location, null);

                role.Location.Add(location.GetResourceReference());
            }

            var existingRole = Bundle?.FirstOrDefault<PractitionerRole>(pr =>
                pr.Location.Matches(role.Location) 
                && pr.Specialty.Matches(role.Specialty) 
                && pr.Practitioner.Matches(role.Practitioner));
            
            if (existingRole == null)
            { 
                Bundle?.AddResourceEntry(role, null);
            }

            return practitioner;
        }
    }
}