using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Model.DataType
{
    public static class CodingExtension
    {
        public static Coding FromXml(this Coding coding, XElement element)
        {
            if (element == null)
                return null;

            var code = element.Attribute("code")?.Value;
            var system = element.Attribute("codeSystem")?.Value;
            var display = element.Attribute("displayName")?.Value;

            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(system))
                return null;

            var systemUri = Constant.KnownCodeSystemList.FirstOrDefault(c => c.Oid == system)?.Uri;

            if (string.IsNullOrEmpty(systemUri))
                systemUri = "urn:oid:" + system;

            coding.Code = code;
            coding.System = systemUri;
            coding.Display = display;

            return coding;
        }
    }
}