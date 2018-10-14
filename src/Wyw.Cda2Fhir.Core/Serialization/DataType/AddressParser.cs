using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Serialization.DataType
{
    public class AddressParser
    {
        public Address FromXml(XElement element)
        {
            var use = element?.Attribute("use")?.Value;

            if (string.IsNullOrEmpty(use))
                return null;

            var addr = new Address();

            switch (use)
            {
                case "H":
                case "HP":
                case "HV":
                    addr.Use = Address.AddressUse.Home;
                    break;

                case "WP":
                case "DIR":
                case "PUB":
                    addr.Use = Address.AddressUse.Work;
                    break;

                case "TMP":
                    addr.Use = Address.AddressUse.Temp;
                    break;

                case "OLD":
                    addr.Use = Address.AddressUse.Old;
                    break;

                default:
                    return null;

            }

            foreach (var child in element.Elements())
            {
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
            }

            return addr;
        }
    }
}
