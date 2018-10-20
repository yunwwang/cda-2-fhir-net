using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Extension;

namespace Wyw.Cda2Fhir.Core.Serialization.DataType
{
    public class CodeableConceptParser : BaseParser
    {
        public CodeableConcept FromXml(XElement element)
        {
            if (element == null)
                return null;

            var codeableConcept = new CodeableConcept();

            var coding = new CodingParser().FromXml(element);

            if (coding != null)
                codeableConcept.Coding.Add(coding);

            var transElements = element.CdaElements("translation");

            foreach (var transElement in transElements)
            {
                coding = new CodingParser().FromXml(transElement);

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
