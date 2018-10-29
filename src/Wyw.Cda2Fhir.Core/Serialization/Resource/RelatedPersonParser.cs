using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Extension;
using Wyw.Cda2Fhir.Core.Model;
using Wyw.Cda2Fhir.Core.Serialization.DataType;

namespace Wyw.Cda2Fhir.Core.Serialization.Resource
{
    public class RelatedPersonParser : BaseParser<RelatedPerson>
    {
        public RelatedPersonParser()
        {
        }

        public RelatedPersonParser(Bundle bundle) : base(bundle)
        {
        }

        public override RelatedPerson FromXml(XElement element)
        {
            if (element == null)
                return null;

            var relatedPerson = new RelatedPerson
            {
                Id = Guid.NewGuid().ToString()
            };

            var classCode = element.Attribute("classCode")?.Value;

            if (!string.IsNullOrEmpty(classCode))
                relatedPerson.Relationship = new CodeableConcept
                {
                    Coding = new List<Coding>
                    {
                        new Coding("urn:oid:2.16.840.1.113883.1.11.19563", classCode)
                    }
                };

            foreach (var child in element.Elements())
                switch (child.Name.LocalName)
                {
                    case "code":
                        relatedPerson.Relationship = FromXml(new CodeableConceptParser(), child);
                        break;

                    case "addr":
                        var addr = FromXml(new AddressParser(), child);
                        if (addr != null)
                            relatedPerson.Address.Add(addr);
                        break;

                    case "telecom":
                        var contactPoint = FromXml(new ContactPointParser(), child);
                        if (contactPoint != null)
                            relatedPerson.Telecom.Add(contactPoint);
                        break;

                    case "guardianPerson":
                    case "relatedPerson":
                    case "associatedPerson":
                        var name = FromXml(new HumanNameParser(), child.CdaElement("name"));
                        if (name != null)
                            relatedPerson.Name.Add(name);
                        break;
                }

            if (!relatedPerson.Name.Any())
                Errors.Add(ParserError.CreateParseError(element, "does NOT have name element",
                    ParseErrorLevel.Warning));

            var existingPerson = Bundle?.FirstOrDefault<RelatedPerson>(p => p.Name.IsExactly(relatedPerson.Name));

            if (existingPerson != null)
                return existingPerson;

            Bundle?.AddResourceEntry(relatedPerson, null);

            return relatedPerson;
        }
    }
}