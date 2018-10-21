﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Extension;
using Wyw.Cda2Fhir.Core.Model;
using Wyw.Cda2Fhir.Core.Serialization;
using Wyw.Cda2Fhir.Core.Serialization.DataType;
using Wyw.Cda2Fhir.Core.Serialization.Rersource;

namespace Wyw.Cda2Fhir.Core
{
    public class CdaParser : BaseParser<Bundle>
    {
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
                Id = Guid.NewGuid().ToString()
            };

            var header = new Composition
            {
                Id = Guid.NewGuid().ToString(),
                Meta = new Meta()
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
                    var confidentialityCode = new CodeParser().FromXml(child, Errors)?.Value;
                    if (!string.IsNullOrEmpty(confidentialityCode) && Enum.TryParse(confidentialityCode, true,
                            out Composition.ConfidentialityClassification confidentialityClassification))
                        header.Confidentiality = confidentialityClassification;
                }
                else if (child.Name.LocalName == "languageCode")
                {
                    header.Language = new CodeParser().FromXml(child, Errors)?.Value;
                }
                else if (child.Name.LocalName == "setId")
                {
                    header.Identifier = new IdentifierParser().FromXml(child, Errors);
                }
                else if (child.Name.LocalName == "recordTarget")
                {
                    var patientRole = child.CdaElement("patientRole");
                    var patient = new PatientParser(bundle).FromXml(patientRole, Errors);
                    if (patient != null)
                    {
                        header.Subject = new ResourceReference($"{patient.TypeName}/{patient.Id}");
                        bundle.Entry.Add(new Bundle.EntryComponent {Resource = patient});
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