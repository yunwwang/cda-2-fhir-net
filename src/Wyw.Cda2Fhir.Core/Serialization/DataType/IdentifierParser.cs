using System.Xml;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Sprache;
using Wyw.Cda2Fhir.Core.Model;

namespace Wyw.Cda2Fhir.Core.Serialization.DataType
{
    public class IdentifierParser : BaseParser
    {
        public ParseResult FromXml(XElement element)
        {
            if (element == null)
                return null;

            var system = element.Attribute("root")?.Value;
            var value = element.Attribute("extension")?.Value;

            if (!string.IsNullOrEmpty(system))
            {
                system = "urn:oid:" + system;
            }

            Result.Resource = new Identifier(system, value);
            return Result;
        }
    }
}
