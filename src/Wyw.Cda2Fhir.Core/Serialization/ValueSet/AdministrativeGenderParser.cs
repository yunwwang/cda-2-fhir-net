using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Serialization.ValueSet
{
    public class AdministrativeGenderParser
    {
        public AdministrativeGender? FromCda(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            switch (value)
            {
                case "M":
                    return AdministrativeGender.Male;
                case "F":
                    return AdministrativeGender.Female;
                case "UN":
                    return AdministrativeGender.Other;
                case "UNK":
                    return AdministrativeGender.Unknown;
                default:
                    return null;
            }
        }
    }
}