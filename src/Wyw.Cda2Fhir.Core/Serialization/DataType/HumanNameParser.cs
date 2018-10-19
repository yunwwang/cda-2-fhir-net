using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Serialization.ValueSet;

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
                Use = new NameUseParser().FromCda(element.Attribute("use")?.Value)
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

            // Name shall have Family and Given
            if (string.IsNullOrEmpty(name.Family) || !name.Given.Any())
                return null;

            return name;
        }
    }
}