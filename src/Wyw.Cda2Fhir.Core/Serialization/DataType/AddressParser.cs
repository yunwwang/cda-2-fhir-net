using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Model;
using Wyw.Cda2Fhir.Core.Serialization.ValueSet;

namespace Wyw.Cda2Fhir.Core.Serialization.DataType
{
    public class AddressParser : BaseParser
    {
        public ParseResult FromXml(XElement element)
        {
            if (element == null)
                return null;

            var addr = new Address
            {
                Use = new AddressUseParser().FromCda(element.Attribute("use")?.Value)
            };

            foreach (var child in element.Elements())
                switch (child.Name.LocalName)
                {
                    case "country":
                        addr.Country = child.Value;
                        break;

                    case "state":
                        addr.State = child.Value;
                        break;

                    case "city":
                        addr.City = child.Value;
                        break;

                    case "postalCode":
                        addr.PostalCode = child.Value;
                        break;

                    case "streetAddressLine":
                        addr.LineElement.Add(new FhirString(child.Value));
                        break;
                }

            Result.Resource = addr;

            return Result;
        }
    }
}