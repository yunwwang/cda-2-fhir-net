using System;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Extension;
using Wyw.Cda2Fhir.Core.Serialization.DataType;

namespace Wyw.Cda2Fhir.Core.Serialization.Rersource
{
    public class RelatedPersonParser
    {
        public RelatedPerson FromXml(XElement element)
        {
            if (element == null)
                return null;

            var relatedPerson = new RelatedPerson
            {
                Id = Guid.NewGuid().ToString()
            };

            foreach (var child in element.Elements())
                switch (child.Name.LocalName)
                {
                    case "code":
                        relatedPerson.Relationship = new CodeableConceptParser().FromXml(child);
                        break;

                    case "addr":
                        var addr = new AddressParser().FromXml(child);
                        if (addr != null)
                            relatedPerson.Address.Add(addr);
                        break;

                    case "telecom":
                        var contactPoint = new ContactPointParser().FromXml(child);
                        if (contactPoint != null)
                            relatedPerson.Telecom.Add(contactPoint);
                        break;

                    case "guardianPerson":
                        var name = new HumanNameParser().FromXml(child.CdaElement("name"));
                        if (name != null)
                            relatedPerson.Name.Add(name);
                        break;
                }

            return relatedPerson;
        }
    }
}