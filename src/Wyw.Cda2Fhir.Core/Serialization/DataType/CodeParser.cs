using Hl7.Fhir.Model;
using System.Xml.Linq;
using Wyw.Cda2Fhir.Core.Model;

namespace Wyw.Cda2Fhir.Core.Serialization.DataType
{
    public class CodeParser : BaseParser<Code>
    {
        public override Code FromXml(XElement element)
        {
            var value = element?.Attribute("code")?.Value;

            return string.IsNullOrWhiteSpace(value) ? null : new Code(value);
        }
    }
}