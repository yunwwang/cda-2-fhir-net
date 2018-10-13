using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Model.DataType
{
    public static class CodeExtension
    {
        public static Code FromXml(this Code code, XElement element)
        {
            if (element == null)
                return null;

            var value = element.Attribute("code")?.Value;

            if (string.IsNullOrWhiteSpace(value))
                return null;

            code.Value = value;

            return code;
        }
    }
}