using System.Xml.Linq;
using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Serialization.DataType
{
    public class HumanNameParser
    {
        public HumanName FromXml(XElement element)
        {
            if (element == null)
                return null;

            var name = new HumanName
            {
                Use = new NameUseParser().FromXml(element.Attribute("use"))
            };

            foreach (var child in element.Elements())
                switch (child.Name.LocalName)
                {
                    case "family":
                        name.Family = child.Value;
                        break;
                    case "given":
                        var given = child.Value;
                        if (!string.IsNullOrEmpty(given))
                            name.GivenElement.Add(new FhirString(given));
                        break;
                    case "prefix":
                        var prefix = child.Value;
                        if (!string.IsNullOrEmpty(prefix))
                            name.PrefixElement.Add(new FhirString(prefix));
                        break;
                    case "suffix":
                        var suffix = child.Value;
                        if (!string.IsNullOrEmpty(suffix))
                            name.SuffixElement.Add(new FhirString(suffix));
                        break;
                }

            return name;
        }
    }

    public class NameUseParser
    {
        public HumanName.NameUse? FromXml(XAttribute attribute)
        {
            if (string.IsNullOrEmpty(attribute?.Value))
                return null;

            switch (attribute.Value)
            {
                case "C":
                    return HumanName.NameUse.Official;
                case "L":
                    return HumanName.NameUse.Usual;
                case "A":
                case "I":
                case "P":
                case "R":
                    return HumanName.NameUse.Nickname;
                default:
                    return null;
            }
        }
    }
}