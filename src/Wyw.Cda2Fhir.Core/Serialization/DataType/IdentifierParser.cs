using System.Xml.Linq;
using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Serialization.DataType
{
    public class IdentifierParser
    {
        public Identifier FromXml(XElement element)
        {
            if (element == null)
                return null;

            var system = element.Attribute("root")?.Value;
            var value = element.Attribute("extension")?.Value;

            if (string.IsNullOrEmpty(system))
                return null;

            //if (!system.StartsWith("http") && !system.StartsWith("urn:oid:"))
            system = "urn:oid:" + system;

            return new Identifier(system, value);
        }
    }
}
