using System;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Extension;
using Wyw.Cda2Fhir.Core.Serialization;
using Wyw.Cda2Fhir.Core.Serialization.DataType;

namespace Wyw.Cda2Fhir.Core
{
    public class CdaParser : BaseParser
    {
        public Bundle Convert(XDocument xml)
        {
            var rootElement = xml?.Root;

            if (rootElement == null)
                throw new ArgumentNullException(nameof(xml));

            // Not a CDA xml
            if (rootElement.Name.LocalName != "ClinicalDocument")
                return null;

            var bundle = new Bundle
            {
                Id = Guid.NewGuid().ToString()
            };

            var header = new Composition
            {
                Id = Guid.NewGuid().ToString()
            };

            bundle.Entry.Add(new Bundle.EntryComponent {Resource = header});

            foreach (var child in rootElement.Elements())
                switch (child.Name.LocalName)
                {
                    case "templateId":
                        var templateId = new IdentifierParser().FromXml(child)?.System;
                        if (!string.IsNullOrEmpty(templateId))
                        {
                            if (header.Meta == null)
                                header.Meta = new Meta();

                            if (header.Meta.ProfileElement.All(p => p.Value != templateId))
                                header.Meta.ProfileElement.Add(new FhirUri(templateId));
                        }

                        break;

                    case "id":
                        bundle.Identifier = new IdentifierParser().FromXml(child);
                        break;
                    case "code":
                        header.Type = new CodeableConceptParser().FromXml(child);
                        break;
                    case "title":
                        header.Title = child.Value;
                        break;
                    case "effectiveTime":
                        header.DateElement = new FhirDateTimeParser().FromXml(child);
                        break;
                    case "confidentialityCode":
                        var confidentialityCode = new CodeParser().FromXml(child)?.Value;
                        if (!string.IsNullOrEmpty(confidentialityCode) && Enum.TryParse(confidentialityCode, true,
                                out Composition.ConfidentialityClassification confidentialityClassification))
                            header.Confidentiality = confidentialityClassification;
                        break;
                    case "languageCode":
                        header.Language = new CodeParser().FromXml(child)?.Value;
                        break;
                    case "setId":
                        header.Identifier = new IdentifierParser().FromXml(child);
                        break;
                    case "recordTarget":
                        var patientRole = child.CdaElement("patientRole");
                        var patient = new PatientParser().FromXml(patientRole);
                        if (patient != null)
                        {
                            header.Subject = new ResourceReference("Patient/" + patient.Id);
                            bundle.Entry.Add(new Bundle.EntryComponent {Resource = patient});
                        }

                        break;
                }


            return bundle;
        }
    }
}