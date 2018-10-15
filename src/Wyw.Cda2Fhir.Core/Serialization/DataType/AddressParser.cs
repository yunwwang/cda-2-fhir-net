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

            var addreeUse = new AddressUseParser().FromXml(element.Attribute("use"));

            if (addreeUse == null)
                return null;

            var addr = new Address
            {
                Use = addreeUse
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
}