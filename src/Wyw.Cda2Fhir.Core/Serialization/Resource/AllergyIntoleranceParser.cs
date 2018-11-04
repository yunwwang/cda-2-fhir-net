using System;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Extension;
using Wyw.Cda2Fhir.Core.Serialization.DataType;

namespace Wyw.Cda2Fhir.Core.Serialization.Resource
{
    public class AllergyIntoleranceParser : BaseParser<AllergyIntolerance>
    {
        public AllergyIntoleranceParser() : base()
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
                Id = Guid.NewGuid().ToString(),
                Meta = new Meta()
            };

            ai.Meta.ProfileElement.Add(new FhirUri("http://hl7.org/fhir/us/core/StructureDefinition/us-core-allergyintolerance"));

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
                    case "author":
                        var author = FromXml(new PractitionerParser(Bundle), child.CdaElement("assignedAuthor"));
                        if (author != null)
                            ai.Recorder = author.GetResourceReference();
                        break;
                    case "entryRelationship":
                        AddObservation(ai, child.CdaElement("observation"));
                        break;

                }

            Bundle?.Entry.Add(new Bundle.EntryComponent(){Resource = ai});

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

        private void AddObservation(AllergyIntolerance ai, XElement element)
        {
            if (ai == null || element == null)
                return;

            foreach (var child in element.Elements())
            {
                switch (child.Name.LocalName)
                {
                    case "id":
                        var id = FromXml(new IdentifierParser(), child);
                        if (id != null)
                            ai.Identifier.Add(id);
                        break;
                    case "code":
                        // always "ASSERTION"
                        break;
                    case "statusCode":
                        // always "completed"
                        break;
                    case "effectiveTime":
                        // What is the difference from ACT?
                        break;
                    case "author":
                        var author = FromXml(new PractitionerParser(Bundle), child.CdaElement("assignedAuthor"));
                        if (author != null)
                            ai.Asserter = author.GetResourceReference();
                        ai.AssertedDateElement = FromXml(new FhirDateTimeParser(), child.CdaElement("time"));
                        break;
                }
            }
        }
    }
}