using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace Wyw.Cda2Fhir.Core.Model
{
    public class ParseError
    {
        public ParseErrorLevel ErrorLevel { get; set; }
        public string Message { get; set; }

        public ParseError()
        {
        }

        public ParseError(string message, ParseErrorLevel level)
        {
            Message = message;
            ErrorLevel = level;
        }

        public static ParseError CreateParseError(XElement element, string message, ParseErrorLevel level)
        {
            var lineNuber = ((IXmlLineInfo)element).LineNumber;

            message = $"Line {lineNuber}: <{element.Name.LocalName}> {message}";

            return new ParseError(message, level);
        }
    }

    public enum ParseErrorLevel
    {
        Warning,
        Error
    }

}
