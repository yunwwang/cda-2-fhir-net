using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Serialization.ValueSet;

namespace Wyw.Cda2Fhir.Core.Serialization.DataType
{
    public class CodingParser
    {
        public Coding FromXml(XElement element)
        {
            if (element == null)
                return null;

            var code = element.Attribute("code")?.Value;
            var system = element.Attribute("codeSystem")?.Value;
            var display = element.Attribute("displayName")?.Value;

            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(system))
                return null;

            var systemUri = new CodeSystemParser().FromCda(system);

            return new Coding(systemUri, code, display);
        }
    }
}