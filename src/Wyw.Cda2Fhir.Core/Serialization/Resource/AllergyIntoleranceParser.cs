using System;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Extension;
using Wyw.Cda2Fhir.Core.Serialization.DataType;

namespace Wyw.Cda2Fhir.Core.Serialization.Resource
{
    public class AllergyIntoleranceParser : BaseParser<AllergyIntolerance>
    {
        public AllergyIntoleranceParser()
        {
        }

        public AllergyIntoleranceParser(Bundle bundle) : base(bundle)
        {
        }

        public override AllergyIntolerance FromXml(XElement element)
        {
            if (element == null)
                return null;

            var ai = new AllergyIntolerance
            {
                Id = Guid.NewGuid().ToString()
            };

            foreach(var child in element.Elements())
                switch (child.Name.LocalName)
                {
                    case "id":
                        var id = FromXml(new IdentifierParser(), child);
                        if (id != null)
                            ai.Identifier.Add(id);
                        break;
                    case "statusCode":
                        AddStatusCode(ai, child);
                        break;
                    case "effectiveTime":
                        ai.Onset = FromXml(new FhirDateTimeParser(), child.CdaElement("low"));
                        ai.LastOccurrenceElement = FromXml(new FhirDateTimeParser(), child.CdaElement("high"));
                        break;
                }

            return ai;
        }

        private void AddStatusCode(AllergyIntolerance ai, XElement element)
        {
            if (ai == null || element == null)
                return;

            switch (element.Attribute("code")?.Value)
            {
                case "active":
                    ai.ClinicalStatus = AllergyIntolerance.AllergyIntoleranceClinicalStatus.Active;
                    ai.VerificationStatus = AllergyIntolerance.AllergyIntoleranceVerificationStatus.Confirmed;
                    break;

                case "completed":
                    ai.ClinicalStatus = AllergyIntolerance.AllergyIntoleranceClinicalStatus.Resolved;
                    ai.VerificationStatus = AllergyIntolerance.AllergyIntoleranceVerificationStatus.Confirmed;
                    break;

                case "aborted":
                    ai.ClinicalStatus = AllergyIntolerance.AllergyIntoleranceClinicalStatus.Inactive;
                    ai.VerificationStatus = AllergyIntolerance.AllergyIntoleranceVerificationStatus.EnteredInError;
                    break;

                case "suspended":
                    ai.ClinicalStatus = AllergyIntolerance.AllergyIntoleranceClinicalStatus.Inactive;
                    ai.VerificationStatus = AllergyIntolerance.AllergyIntoleranceVerificationStatus.Unconfirmed;
                    break;

            }
        }
    }
}