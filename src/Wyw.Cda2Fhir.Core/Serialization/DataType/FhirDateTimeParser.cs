using System;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Model;

namespace Wyw.Cda2Fhir.Core.Serialization.DataType
{
    public class FhirDateTimeParser : BaseParser<FhirDateTime>
    {
        public override FhirDateTime FromXml(XElement element)
        {
            if (element == null)
                return null;

            var timeString = element.Attribute("value")?.Value;

            if (timeString == null)
            {
                Errors.Add(ParserError.CreateParseError(element, "does NOT have value attribute",
                    ParseErrorLevel.Error));
                return null;
            }

            if (timeString.Length < 4)
            {
                Errors.Add(ParserError.CreateParseError(element, "does NOT have valid value attribute",
                    ParseErrorLevel.Error));
                return null;
            }

            var format = string.Empty;

            var timeZoneIndex = timeString.IndexOfAny(new char[] { '-', '+' });

            if (timeZoneIndex == -1)
                timeZoneIndex = timeString.Length;

            switch (timeZoneIndex)
            {
                case 4:
                    format = "yyyy";
                    break;
                case 6:
                    format = "yyyyMM";
                    break;
                case 8:
                    format = "yyyyMMdd";
                    break;
                case 10:
                    format = "yyyyMMddHH";
                    break;
                case 12:
                    format = "yyyyMMddHHmm";
                    break;
                case 14:
                    format = "yyyyMMddHHmmss";
                    break;
                case 19:
                    format = "yyyyMMddHHmmss.ffff";
                    break;
            }


            if (timeZoneIndex == timeString.Length - 3)
                format += "zz";
            else if (timeZoneIndex == timeString.Length - 5)
            {
                format += "zzz";
                timeString = timeString.Insert(timeString.Length - 2, ":");
            }

            DateTime datetime = DateTime.ParseExact(timeString, format, null).ToUniversalTime();

            return new FhirDateTime(datetime);
        }
    }
}
