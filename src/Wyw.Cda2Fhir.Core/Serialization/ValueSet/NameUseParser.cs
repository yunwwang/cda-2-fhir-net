using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Serialization.ValueSet
{
    public class NameUseParser
    {
        public HumanName.NameUse? FromCda(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            switch (value)
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