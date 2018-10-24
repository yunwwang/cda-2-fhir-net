using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Specification.Source;
using Hl7.Fhir.Validation;
using Wyw.Cda2Fhir.Core.Extension;
using Wyw.Cda2Fhir.Core.Model;
using Wyw.Cda2Fhir.Core.Serialization;
using Wyw.Cda2Fhir.Core.Serialization.DataType;
using Wyw.Cda2Fhir.Core.Serialization.Resource;

namespace Wyw.Cda2Fhir.Core
{
    public class CdaParser : BaseParser<Bundle>
    {
        public CdaParser()
        {
        }

        public CdaParser(CdaParserSettings settings)
        {
            ParserSettings = settings;
        }

        public CdaParserSettings ParserSettings { get; set; }

        public Bundle Convert(XDocument xml)
        {
            return FromXml(xml?.Root);
        }

        public override Bundle FromXml(XElement rootElement)
        {
            if (rootElement == null)
            {
                Errors.Add(new ParserError("XML Document doesn't have a root", ParseErrorLevel.Error));
                return null;
            }

            // Not a CDA xml
            if (rootElement.Name.LocalName != "ClinicalDocument")
            {
                Errors.Add(new ParserError("XML Document's root is not ClinicalDocument", ParseErrorLevel.Error));
                return null;
            }


            Bundle = new Bundle
            {
                Id = Guid.NewGuid().ToString(),
                Type = Bundle.BundleType.Document
            };

            var header = new Composition
            {
                Id = Guid.NewGuid().ToString(),
                Meta = new Meta(),
                Status = CompositionStatus.Final
            };

            Bundle.AddResourceEntry(header, null);

            foreach (var child in rootElement.Elements())
                switch (child.Name.LocalName)
                {
                    case "id":
                        Bundle.Identifier = FromXml(new IdentifierParser(), child);
                        break;
                    case "code":
                        AddCode(header, child);
                        break;
                    case "title":
                        header.Title = child.Value;
                        break;
                    case "effectiveTime":
                        header.DateElement = FromXml(new FhirDateTimeParser(), child);
                        break;
                    case "confidentialityCode":
                        var confidentialityCode = FromXml(new CodeParser(), child)?.Value;
                        if (!string.IsNullOrEmpty(confidentialityCode) && Enum.TryParse(confidentialityCode, true,
                                out Composition.ConfidentialityClassification confidentialityClassification))
                            header.Confidentiality = confidentialityClassification;
                        break;
                    case "languageCode":
                        header.Language = FromXml(new CodeParser(), child)?.Value;
                        break;
                    case "setId":
                        header.Identifier = FromXml(new IdentifierParser(), child);
                        break;
                    case "recordTarget":
                        var patient = FromXml(new PatientParser(Bundle), child.CdaElement("patientRole"));
                        if (patient != null)
                            header.Subject = patient.GetResourceReference();
                        break;
                    case "author":
                        AddAuthor(header, child);
                        break;
                    case "dataEnterer":
                        var dataEnterer = FromXml(new PractitionerParser(Bundle), child.CdaElement("assignedEntity"));
                        if (dataEnterer != null)
                            header.AddExtension(
                                "http://hl7.org/fhir/us/ccda/StructureDefinition/CCDA-on-FHIR-Data-Enterer",
                                dataEnterer.GetResourceReference());
                        break;
                    case "informant":
                        AddInformant(header, child);
                        break;
                    case "custodian":
                        var custodian = FromXml(new OrganizationParser(Bundle),
                            child.CdaElement("assignedCustodian")
                                ?.CdaElement("representedCustodianOrganization"));

                        if (custodian != null)
                            header.Custodian = custodian.GetResourceReference();
                        break;
                    case "informationRecipient":
                        var infoRecipient = FromXml(new PractitionerParser(Bundle),
                            child.CdaElement("intendedRecipient"));
                        if (infoRecipient != null)
                            header.AddExtension(
                                "http://hl7.org/fhir/us/ccda/StructureDefinition/CCDA-on-FHIR-Information-Recipient",
                                infoRecipient.GetResourceReference());
                        break;
                    case "legalAuthenticator":
                        AddAuthenticator(header, child, Composition.CompositionAttestationMode.Legal);
                        break;
                    case "authenticator":
                        AddAuthenticator(header, child,
                            Composition.CompositionAttestationMode.Professional);
                        break;
                }

            if (ParserSettings.RunValidation)
            {
                var settings = new ValidationSettings
                {
                    ResourceResolver = new CachedResolver(new MultiResolver(
                        new ZipSource("Definitions/stu3-definitions.xml.zip"),
                        new ZipSource("Definitions/us-core-definitions.xml.zip"),
                        new ZipSource("Definitions/ccda-on-fhir-definitions.xml.zip")
                    ))
                };
                var validator = new Validator(settings);

                var outcome = validator.Validate(Bundle);

                foreach (var issue in outcome.Issue)
                    Errors.Add(new ParserError(issue.Details.Text, ParseErrorLevel.Error));
            }

            return Bundle;
        }

        private void AddCode(Composition header, XElement element)
        {
            header.Type = new CodeableConceptParser().FromXml(element, Errors);

            var type = header.Type?.Coding.FirstOrDefault()?.Code;

            if (string.IsNullOrEmpty(type))
                return;

            if (type == "34133-9")
            {
                header.Meta.ProfileElement.Add(new FhirUri(
                    "http://hl7.org/fhir/us/ccda/StructureDefinition/CCDA-on-FHIR-Continuity-of-Care-Document"));
            }
            else
            {
                var templateIdElement = element.Parent.CdaElement("templateId");
                var templateId = new IdentifierParser().FromXml(templateIdElement)?.System;

                if (!string.IsNullOrEmpty(templateId) && header.Meta.ProfileElement.All(p => p.Value != templateId))
                    header.Meta.ProfileElement.Add(new FhirUri(templateId));
            }
        }

        private void AddAuthor(Composition header, XElement element)
        {
            var assignedAuthorElement = element.CdaElement("assignedAuthor");

            Resource author = null;

            if (assignedAuthorElement.CdaElement("assignedPerson") != null)
                author = FromXml(new PractitionerParser(Bundle), assignedAuthorElement);
            else if (assignedAuthorElement.CdaElement("assignedAuthoringDevice") != null)
                author = FromXml(new DeviceParser(Bundle), assignedAuthorElement);
            else
                Errors.Add(ParserError.CreateParseError(assignedAuthorElement,
                    "does NOT have either assignedPerson element or assignedAuthoringDevice element",
                    ParseErrorLevel.Error));

            if (author != null)
                header.Author.Add(new ResourceReference($"{author.TypeName}/{author.Id}"));
        }

        private void AddInformant(Composition header, XElement element)
        {
            foreach (var entity in element.Elements())
                if (entity.Name.LocalName == "assignedEntity")
                {
                    var practitioner = FromXml(new PractitionerParser(Bundle), entity);

                    if (practitioner != null)
                        header.AddExtension("http://hl7.org/fhir/us/ccda/StructureDefinition/CCDA-on-FHIR-Informant",
                            practitioner.GetResourceReference());
                }
                else if (entity.Name.LocalName == "relatedEntity")
                {
                    var relatedPerson = FromXml(new RelatedPersonParser(Bundle), entity);

                    if (relatedPerson != null)
                    {
                        relatedPerson.Patient = header.Subject;
                        header.AddExtension("http://hl7.org/fhir/us/ccda/StructureDefinition/CCDA-on-FHIR-Informant",
                            relatedPerson.GetResourceReference());
                    }
                }
        }

        private void AddAuthenticator(Composition header, XElement element, Composition.CompositionAttestationMode mode)
        {
            var authenticator = FromXml(new PractitionerParser(Bundle), element.CdaElement("assignedEntity"));
            if (authenticator != null)
                header.Attester.Add(new Composition.AttesterComponent
                {
                    Mode = new List<Composition.CompositionAttestationMode?> {mode},
                    Party = authenticator.GetResourceReference()
                });
        }
    }
}