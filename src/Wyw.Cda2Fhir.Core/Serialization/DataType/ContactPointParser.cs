using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Serialization.ValueSet;

namespace Wyw.Cda2Fhir.Core.Serialization.DataType
{
    public class ContactPointParser
    {
        public ContactPoint FromXml(XElement element)
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
                    value = value.Substring(4);

                contactPoint.Value = value;
            }

            return contactPoint;
        }
    }
}