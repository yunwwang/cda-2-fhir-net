using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Serialization.ValueSet;

namespace Wyw.Cda2Fhir.Core.Serialization.DataType
{
    public class ContactPointParser :BaseParser<ContactPoint>
    {
        public override ContactPoint FromXml(XElement element)
        {
            if (element == null)
                return null;

            var contactPoint = new ContactPoint
            {
                Use = new ContactPointUseParser().FromCda(element.Attribute("use")?.Value)
            };

            var value = element.Attribute("value")?.Value;

            if (!string.IsNullOrEmpty(value))
            {
                if (value.StartsWith("tel:"))
                {
                    contactPoint.Value = value.Substring(4);
                    contactPoint.System = ContactPoint.ContactPointSystem.Phone;
                }
                else if (value.StartsWith("fax:"))
                {
                    contactPoint.Value = value.Substring(4);
                    contactPoint.System = ContactPoint.ContactPointSystem.Fax;
                }
                else if (value.StartsWith("mailto:"))
                {
                    contactPoint.Value = value.Substring(7);
                    contactPoint.System = ContactPoint.ContactPointSystem.Email;
                }
                else if (value.StartsWith("http:"))
                {
                    contactPoint.Value = value.Substring(5);
                    contactPoint.System = ContactPoint.ContactPointSystem.Url;
                }
                else
                {
                    contactPoint.Value = value;
                    contactPoint.System = ContactPoint.ContactPointSystem.Other;
                }
            }

            return contactPoint;
        }
    }
}