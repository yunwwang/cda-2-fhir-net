using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Serialization.DataType
{
    public class FhirBooleanParser : BaseParser<FhirBoolean>
    {
        public override FhirBoolean FromXml(XElement element)
        {
            var value = element?.Attribute("value")?.Value;

            return bool.TryParse(value, out bool result) ? new FhirBoolean(result) : null;
        }
    }
}
