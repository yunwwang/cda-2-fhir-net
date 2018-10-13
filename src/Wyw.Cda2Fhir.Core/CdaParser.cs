using System;
using System.Collections.Generic;
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
                }
            }


            return bundle;
        }
    }
}
