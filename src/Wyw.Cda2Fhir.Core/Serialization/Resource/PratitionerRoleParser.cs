using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Wyw.Cda2Fhir.Core.Serialization.DataType;

namespace Wyw.Cda2Fhir.Core.Serialization.Resource
{
    public class PratitionerRoleParser : BaseParser<PractitionerRole>
    {
        public override PractitionerRole FromXml(XElement element)
        {
            if (element == null)
                return null;

            var role = new PractitionerRole
            {
                Id = Guid.NewGuid().ToString()
            };

            var code = FromXml(new CodeableConceptParser(), element);

            if (code != null)
                role.Specialty.Add(code);

            return role;
        }
    }
}
