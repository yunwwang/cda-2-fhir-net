using System.Xml;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Sprache;
using Wyw.Cda2Fhir.Core.Model;

namespace Wyw.Cda2Fhir.Core.Serialization.DataType
{
    public class IdentifierParser : BaseParser<Identifier>
    {
        public override Identifier FromXml(XElement element)
        {
            if (element == null)
                return null;

            var system = element.Attribute("root")?.Value;
            var value = element.Attribute("extension")?.Value;

            if (string.IsNullOrEmpty(system))
            {
                Errors.Add(ParserError.CreateParseError(element, "does NOT have root attribute", ParseErrorLevel.Error));
                return null;
            }

            if (string.IsNullOrEmpty(value))
            {
                Errors.Add(ParserError.CreateParseError(element, "does NOT have extension attribute", ParseErrorLevel.Warning));
                return null;
            }

            system = "urn:oid:" + system;

            return new Identifier(system, value);
        }
    }
}
