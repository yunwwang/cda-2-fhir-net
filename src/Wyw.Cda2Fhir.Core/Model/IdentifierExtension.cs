using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;
using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Model
{
    public static class IdentifierExtension
    {
        public static Identifier FromXml(this Identifier id, XElement element)
        {
            if (element == null)
                return null;

            id.System = element.Attribute("root")?.Value;
            id.Value = element.Attribute("extension")?.Value;

            if (string.IsNullOrEmpty(id.System) || string.IsNullOrEmpty(id.Value))
                return null;

            return id;
        }
    }
}
