using Hl7.Fhir.Model;
using System.Xml.Linq;

namespace Wyw.Cda2Fhir.Core.Serialization.DataType
{
    public class CodeParser
    {
        public Code FromXml(XElement element)
        {
            if (element == null)
                return null;

            var value = element.Attribute("code")?.Value;

            return string.IsNullOrWhiteSpace(value) ? null : new Code(value);
        }
    }
}