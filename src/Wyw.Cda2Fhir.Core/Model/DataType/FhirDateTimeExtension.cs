using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Model.DataType
{
    public static class FhirDateTimeExtension
    {
        public static FhirDateTime FromXml(this FhirDateTime dateTime, XElement element)
        {
            var timeString = element?.Attribute("value")?.Value;

            if (timeString == null || timeString.Length < 4)
                return null;

            var timeZoneIndex = timeString.Length;

            var sb = new StringBuilder();

            for (int i = 0; i < timeString.Length; i++)
            {
                if (i == 8)
                    // Add Hour
                    sb.Append("T");
                else if (timeString[i] == '+' || timeString[i] == '-')
                {
                    // Add TimeZone
                    sb.Append("Z");
                    timeZoneIndex = i;
                }
                else if (i == 10 || i == 12 || i == timeZoneIndex + 3)
                    // Add Minute / Second
                    sb.Append(":");

                sb.Append(timeString[i]);
            }
            
            dateTime.Value = sb.ToString();
            return dateTime;
        }
    }
}
