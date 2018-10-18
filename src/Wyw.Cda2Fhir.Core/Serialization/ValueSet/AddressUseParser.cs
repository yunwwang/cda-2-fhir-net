using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Serialization.ValueSet
{
    public class AddressUseParser
    {
        public Address.AddressUse? FromCda(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            switch (value)
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