using System.Xml.Linq;
using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Serialization.DataType
{
    public class AddressParser
    {
        public Address FromXml(XElement element)
        {
            if (element == null)
                return null;

            var addr = new Address
            {
                Use = new AddressUseParser().FromXml(element.Attribute("use"))
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

            return addr;
        }
    }

    public class AddressUseParser
    {
        public Address.AddressUse? FromXml(XAttribute attribute)
        {
            if (string.IsNullOrEmpty(attribute?.Value))
                return null;

            switch (attribute.Value)
            {
                case "H":
                case "HP":
                case "HV":
                    return Address.AddressUse.Home;

                case "WP":
                case "DIR":
                case "PUB":
                    return Address.AddressUse.Work;

                case "TMP":
                    return Address.AddressUse.Temp;

                case "OLD":
                    return Address.AddressUse.Old;

                default:
                    return null;
            }
        }
    }

}