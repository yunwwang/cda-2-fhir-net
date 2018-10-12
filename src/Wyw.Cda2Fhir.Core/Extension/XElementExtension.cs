using System.Collections.Generic;
using System.Xml.Linq;

namespace Wyw.Cda2Fhir.Core.Extension
{
    public static class XElementExtension
    {
        private const string V3Namespace = "{urn:hl7-org:v3}";
        public const string XmlSchemaInstanceNamespace = "{http://www.w3.org/2001/XMLSchema-instance}";

        public static XElement CdaElement(this XElement element, string name)
        {
            return element?.Element(V3Namespace + name);
        }

        public static IEnumerable<XElement> CdaElements(this XElement element, string name)
        {
            return element?.Elements(V3Namespace + name);
        }

        public static IEnumerable<XElement> CdaDescendants(this XElement element, string name)
        {
            return element?.Descendants(V3Namespace + name);
        }
    }
}
