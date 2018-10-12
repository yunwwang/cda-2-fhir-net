using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Extension;

namespace Wyw.Cda2Fhir.Core.Model.DataType
{
    public static class CodeableConceptExtension
    {
        public static CodeableConcept FromXml(this CodeableConcept codeableConcept, XElement element)
        {
            if (element == null)
                return null;
            
            var coding = new Coding().FromXml(element);

            if (coding == null)
                return null;

            codeableConcept.Coding.Add(coding);

            var transElements = element.CdaElements("translation");

            foreach (var transElement in transElements)
            {
                coding = new Coding().FromXml(transElement);

                if (coding == null)
                    continue;

                codeableConcept.Coding.Add(coding);
            }

            codeableConcept.Text = element.CdaElement("originalText")?.CdaElement("reference")?.Attribute("value")
                ?.Value.Trim();

            return codeableConcept;
        }
    }
}
