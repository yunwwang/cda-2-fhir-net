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

            if (!string.IsNullOrEmpty(system))
                system = "urn:oid:" + system;
            else
                Errors.Add(ParseError.CreateParseError(element, "does NOT have root attribute", ParseErrorLevel.Warning));

            return new Identifier(system, value);
        }
    }
}
