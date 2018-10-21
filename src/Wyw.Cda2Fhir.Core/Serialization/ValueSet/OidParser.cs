namespace Wyw.Cda2Fhir.Core.Serialization.ValueSet
{
    public class OidParser
    {
        public string FromCda(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            switch (value)
            {
                case "2.16.840.1.113883.5.2":
                    return "http://hl7.org/fhir/v3/MaritalStatus";

                case "2.16.840.1.113883.5.111":
                    return "http://hl7.org/fhir/v3/RoleCode";
                
                case "2.16.840.1.113883.6.1":
                    return "http://loinc.org";

                case "2.16.840.1.113883.6.96":
                    return "http://snomed.info/sct";

                default:
                    return "urn:oid:" + value;
            }
        }
    }
}
