using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Model;
using Wyw.Cda2Fhir.Core.Model.DataType;
using Wyw.Cda2Fhir.Core.Serialization;

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

            var bundle = new Bundle()
            {
                Id = Guid.NewGuid().ToString()
            };

            var header = new Composition
            {
                Id = Guid.NewGuid().ToString()
            };

            bundle.Entry.Add(new Bundle.EntryComponent(){Resource = header});

            foreach (var child in rootElement.Elements())
            {
                switch (child.Name.LocalName)
                {
                    case "templateId":
                        var templateId = new Identifier().FromXml(child, true)?.System;
                        if (!string.IsNullOrEmpty(templateId))
                        {
                            if (header.Meta == null)
                                header.Meta = new Meta();

                            if (header.Meta.ProfileElement.All(p => p.Value != templateId))
                                header.Meta.ProfileElement.Add(new FhirUri(templateId));
                        }
                        break;

                    case "id":
                        bundle.Identifier = new Identifier().FromXml(child);
                        break;
                    case "code":
                        header.Type = new CodeableConcept().FromXml(child);
                        break;
                    case "title":
                        header.Title = child.Value;
                        break;
                    case "effectiveTime":
                        header.DateElement = new FhirDateTime().FromXml(child);
                        break;
                    case "confidentialityCode":
                        var confidentialityCode = new Code().FromXml(child)?.Value;
                        if (!string.IsNullOrEmpty(confidentialityCode) && Enum.TryParse(confidentialityCode, true,
                            out Composition.ConfidentialityClassification confidentialityClassification))
                        {
                            header.Confidentiality = confidentialityClassification;
                        }
                        break;
                    case "languageCode":
                        header.Language = new Code().FromXml(child)?.Value;
                        break;
                    case "setId":
                        header.Identifier = new Identifier().FromXml(child);
                        break;
                }
            }


            return bundle;
        }
    }
}
