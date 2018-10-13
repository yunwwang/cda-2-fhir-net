using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;
using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Model.DataType
{
    public static class IdentifierExtension
    {
        public static Identifier FromXml(this Identifier id, XElement element, bool rootOnly = false)
        {
            if (element == null)
                return null;

            var system = element.Attribute("root")?.Value;
            var value = element.Attribute("extension")?.Value;

            if (string.IsNullOrEmpty(system) || (string.IsNullOrEmpty(value) && !rootOnly))
                return null;

            //if (!system.StartsWith("http") && !system.StartsWith("urn:oid:"))
            system = "urn:oid:" + system;

            id.System = system;
            id.Value = value;

            return id;
        }
    }
}
