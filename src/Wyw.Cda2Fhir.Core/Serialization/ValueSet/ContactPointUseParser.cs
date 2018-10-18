using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Serialization.ValueSet
{
    public class ContactPointUseParser
    {
        public ContactPoint.ContactPointUse? FromCda(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            switch (value)
            {
                case "H":
                case "HP":
                case "HV":
                    return ContactPoint.ContactPointUse.Home;

                case "WP":
                    return ContactPoint.ContactPointUse.Work;

                case "TMP":
                    return ContactPoint.ContactPointUse.Temp;

                case "OLD":
                    return ContactPoint.ContactPointUse.Old;

                case "MC":
                    return ContactPoint.ContactPointUse.Mobile;

                default:
                    return null;
            }
        }
    }
}