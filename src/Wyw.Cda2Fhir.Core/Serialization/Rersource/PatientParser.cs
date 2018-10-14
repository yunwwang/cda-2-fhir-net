using System;
using System.Xml.Linq;
using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Serialization
{
    public class PatientParser : BaseParser
    {
        public Patient FromXml(XElement element)
        {
            if (element == null)
                return null;

            var patient = new Patient()
            {
                Id = Guid.NewGuid().ToString()
            };

            return patient;
        }
    }
}
