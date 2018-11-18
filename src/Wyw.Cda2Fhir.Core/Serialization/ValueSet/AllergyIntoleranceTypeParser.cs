using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Serialization.ValueSet
{
    public class AllergyIntoleranceTypeParser
    {
        public AllergyIntolerance.AllergyIntoleranceType? FromCda(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            switch (value)
            {
                case "414285001":
                case "416098002":
                case "419199007":
                    return AllergyIntolerance.AllergyIntoleranceType.Allergy;
                case "235719002":
                case "418038007":
                case "418471000":
                case "419511003":
                case "420134006":
                case "59037007":
                    return AllergyIntolerance.AllergyIntoleranceType.Intolerance;
                default:
                    return null;
            }
        }
    }
}