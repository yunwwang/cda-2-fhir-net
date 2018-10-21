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
        public CdaParserSettings ParserSettings { get; set; }

        public CdaParser()
        { }

        public CdaParser(CdaParserSettings settings)
        {
            ParserSettings = settings;
        }

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


            var bundle = new Bundle
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

            bundle.Entry.Add(new Bundle.EntryComponent {Resource = header});

            foreach (var child in rootElement.Elements())
                if (child.Name.LocalName == "id")
                {
                    bundle.Identifier = FromXml(new IdentifierParser(), child);
                }
                else if (child.Name.LocalName == "code")
                {
                    AddCode(header, child);
                }
                else if (child.Name.LocalName == "title")
                {
                    header.Title = child.Value;
                }
                else if (child.Name.LocalName == "effectiveTime")
                {
                    header.DateElement = FromXml(new FhirDateTimeParser(), child);
                }
                else if (child.Name.LocalName == "confidentialityCode")
                {
                    var confidentialityCode = FromXml(new CodeParser(), child)?.Value;
                    if (!string.IsNullOrEmpty(confidentialityCode) && Enum.TryParse(confidentialityCode, true,
                            out Composition.ConfidentialityClassification confidentialityClassification))
                        header.Confidentiality = confidentialityClassification;
                }
                else if (child.Name.LocalName == "languageCode")
                {
                    header.Language = FromXml(new CodeParser(), child)?.Value;
                }
                else if (child.Name.LocalName == "setId")
                {
                    header.Identifier = FromXml(new IdentifierParser(), child);
                }
                else if (child.Name.LocalName == "recordTarget")
                {
                    var patient = FromXml(new PatientParser(bundle), child.CdaElement("patientRole"));
                    if (patient != null)
                    {
                        header.Subject = new ResourceReference($"{patient.TypeName}/{patient.Id}");
                        bundle.Entry.Add(new Bundle.EntryComponent { Resource = patient });
                    }
                }
                else if (child.Name.LocalName == "author")
                {
                    var practitioner = FromXml(new PractitionerParser(bundle), child.CdaElement("assignedAuthor"));

                    if (practitioner != null)
                    {
                        header.Author.Add(new ResourceReference($"{practitioner.TypeName}/{practitioner.Id}"));
                        bundle.Entry.Add(new Bundle.EntryComponent { Resource = practitioner });
                    }
                }

            
            if (ParserSettings.RunValidation)
            {
                var settings = new ValidationSettings
                {
                    ResourceResolver = new CachedResolver(new MultiResolver(
                        new ZipSource("Definitions/stu3-definitions.xml.zip"),
                        new ZipSource("Definitions/us-core-definitions.xml.zip"),                        
                        new DirectorySource("Definitions", new DirectorySourceSettings
                        {
                            Mask = "*.xml"
                        })
                    ))
                };
                var validator = new Validator(settings);

                var outcome = validator.Validate(bundle);

                foreach (var issue in outcome.Issue)
                {
                    Errors.Add(new ParserError(issue.Details.Text, ParseErrorLevel.Error));
                }
            }
            return bundle;
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
    }
}