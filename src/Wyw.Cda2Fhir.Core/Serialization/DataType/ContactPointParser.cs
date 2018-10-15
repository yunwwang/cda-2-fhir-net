using System.Xml.Linq;
using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Serialization.DataType
{
    public class ContactPointParser
    {
        public ContactPoint FromXml(XElement element)
        {
            if (element == null)
                return null;

            var cpUse = new ContactPointUseParser().FromXml(element.Attribute("use"));

            if (cpUse == null)
                return null;

            var contactPoint = new ContactPoint
            {
                Use = cpUse
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