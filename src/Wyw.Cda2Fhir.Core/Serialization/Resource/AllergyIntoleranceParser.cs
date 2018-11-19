using System;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Extension;
using Wyw.Cda2Fhir.Core.Serialization.DataType;
using Wyw.Cda2Fhir.Core.Serialization.ValueSet;

namespace Wyw.Cda2Fhir.Core.Serialization.Resource
{
    public class AllergyIntoleranceParser : BaseParser<AllergyIntolerance>
    {
        private AllergyIntolerance AllergyIntolerance { get; set; }

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

            var patient = Bundle?.FirstOrDefault<Patient>(null);

            var AllergyIntolerance = new AllergyIntolerance
            {
                Id = Guid.NewGuid().ToString(),
                Meta = new Meta(),
                Patient = patient?.GetResourceReference()
            };

            AllergyIntolerance.Meta.ProfileElement.Add(new FhirUri("http://hl7.org/fhir/us/core/StructureDefinition/us-core-allergyintolerance"));

            foreach(var child in element.Elements())
                switch (child.Name.LocalName)
                {
                    case "id":
                        var id = FromXml(new IdentifierParser(), child);
                        if (id != null)
                            AllergyIntolerance.Identifier.Add(id);
                        break;
                    case "statusCode":
                        AddStatusCode(child);
                        break;
                    case "effectiveTime":
                        //AllergyIntolerance.Onset = FromXml(new FhirDateTimeParser(), child.CdaElement("low"));
                        //AllergyIntolerance.LastOccurrenceElement = FromXml(new FhirDateTimeParser(), child.CdaElement("high"));
                        break;
                    case "author":
                        var author = FromXml(new PractitionerParser(Bundle), child.CdaElement("assignedAuthor"));
                        if (author != null)
                            AllergyIntolerance.Recorder = author.GetResourceReference();
                        break;
                    case "entryRelationship":
                        AddAllergyObservation(child.CdaElement("observation"));
                        break;

                }

            Bundle?.Entry.Add(new Bundle.EntryComponent(){Resource = AllergyIntolerance});

            return AllergyIntolerance;
        }

        private void AddStatusCode(XElement element)
        {
            if (AllergyIntolerance == null || element == null)
                return;

            switch (element.Attribute("code")?.Value)
            {
                case "active":
                    AllergyIntolerance.ClinicalStatus = AllergyIntolerance.AllergyIntoleranceClinicalStatus.Active;
                    AllergyIntolerance.VerificationStatus = AllergyIntolerance.AllergyIntoleranceVerificationStatus.Confirmed;
                    break;

                case "completed":
                    AllergyIntolerance.ClinicalStatus = AllergyIntolerance.AllergyIntoleranceClinicalStatus.Resolved;
                    AllergyIntolerance.VerificationStatus = AllergyIntolerance.AllergyIntoleranceVerificationStatus.Confirmed;
                    break;

                case "aborted":
                    AllergyIntolerance.ClinicalStatus = AllergyIntolerance.AllergyIntoleranceClinicalStatus.Inactive;
                    AllergyIntolerance.VerificationStatus = AllergyIntolerance.AllergyIntoleranceVerificationStatus.EnteredInError;
                    break;

                case "suspended":
                    AllergyIntolerance.ClinicalStatus = AllergyIntolerance.AllergyIntoleranceClinicalStatus.Inactive;
                    AllergyIntolerance.VerificationStatus = AllergyIntolerance.AllergyIntoleranceVerificationStatus.Unconfirmed;
                    break;

            }
        }

        private void AddObservation(XElement element)
        {
            if (AllergyIntolerance == null || element == null)
                return;

            var templateId = element.CdaElement("templateId").Value;

            switch(templateId)
            {
                case "2.16.840.1.113883.10.20.22.4.7":
                    AddAllergyObservation(element);
                    break;

                case "2.16.840.1.113883.10.20.22.4.8":
                    AddSeverityObservation(element);
                    break;

                case "2.16.840.1.113883.10.20.22.4.9":
                    AddReactionObservation(element);
                    break;
            }
        }

        private void AddAllergyObservation(XElement element)
        {
            if (AllergyIntolerance == null || element == null)
                return;

            foreach (var child in element.Elements())
            {
                switch (child.Name.LocalName)
                {
                    case "id":
                        var id = FromXml(new IdentifierParser(), child);
                        if (id != null)
                            AllergyIntolerance.Identifier.Add(id);
                        break;
                    case "code":
                        // always "ASSERTION"
                        break;
                    case "statusCode":
                        // always "completed"
                        break;
                    case "effectiveTime":
                        AllergyIntolerance.Onset = FromXml(new FhirDateTimeParser(), child.CdaElement("low"));
                        AllergyIntolerance.LastOccurrenceElement = FromXml(new FhirDateTimeParser(), child.CdaElement("high"));
                        break;
                    case "value":
                        AllergyIntolerance.Type = new AllergyIntoleranceTypeParser().FromCda(child.Attribute("code")?.Value);
                        break;
                    case "author":
                        var author = FromXml(new PractitionerParser(Bundle), child.CdaElement("assignedAuthor"));
                        if (author != null)
                            AllergyIntolerance.Asserter = author.GetResourceReference();
                        AllergyIntolerance.AssertedDateElement = FromXml(new FhirDateTimeParser(), child.CdaElement("time"));
                        break;
                    case "participant":
                        AllergyIntolerance.Code = FromXml(new CodeableConceptParser(), child.CdaDescendants("code").FirstOrDefault());
                        break;
                    case "entryRelationship":
                        AddObservation(child.CdaElement("observation"));
                        break;
                }
            }
        }

        private void AddSeverityObservation(XElement element)
        {

        }

        private void AddReactionObservation(XElement element)
        { }
    }
}