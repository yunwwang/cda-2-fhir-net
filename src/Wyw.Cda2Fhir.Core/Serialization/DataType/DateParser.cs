using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Serialization.DataType
{
    public class DateParser
    {
        public Date FromXml(XElement element)
        {
            var dateString = element?.Attribute("value")?.Value;

            if (dateString == null || dateString.Length < 4)
                return null;

            var sb = new StringBuilder();

            for (int i = 0; i < dateString.Length; i++)
            {
                if (i == 4 || i == 6)
                    sb.Append("-");

                sb.Append(dateString[i]);
            }

            return new Date(sb.ToString());
        }
    }
}
