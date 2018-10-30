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

            var root = element.Attribute("root")?.Value;
            var extension = element.Attribute("extension")?.Value;

            if (string.IsNullOrEmpty(root))
            {
                Errors.Add(ParserError.CreateParseError(element, "does NOT have root attribute", ParseErrorLevel.Error));
                return null;
            }

            root = "urn:oid:" + root;

            return string.IsNullOrEmpty(extension) ? new Identifier("urn:ietf:rfc:3986", root) : new Identifier(root, extension);
        }
    }
}
