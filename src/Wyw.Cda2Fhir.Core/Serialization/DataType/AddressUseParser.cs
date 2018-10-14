using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Serialization.DataType
{
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
