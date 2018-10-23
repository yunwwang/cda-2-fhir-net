using System;
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

            Bundle?.AddResourceEntry(relatedPerson, null);

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
                        var name = FromXml(new HumanNameParser(), child.CdaElement("name"));
                        if (name != null)
                            relatedPerson.Name.Add(name);
                        break;
                }

            if (!relatedPerson.Name.Any())
                Errors.Add(ParserError.CreateParseError(element, "does NOT have name element",
                    ParseErrorLevel.Warning));

            return relatedPerson;
        }
    }
}