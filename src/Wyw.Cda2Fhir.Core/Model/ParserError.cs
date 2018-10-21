using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace Wyw.Cda2Fhir.Core.Model
{
    public class ParserError
    {
        public ParseErrorLevel ErrorLevel { get; set; }
        public string Message { get; set; }

        public ParserError()
        {
        }

        public ParserError(string message, ParseErrorLevel level)
        {
            Message = message;
            ErrorLevel = level;
        }

        public static ParserError CreateParseError(XElement element, string message, ParseErrorLevel level)
        {
            var lineNuber = ((IXmlLineInfo)element).LineNumber;

            message = $"Line {lineNuber}: <{element.Name.LocalName}> {message}";

            return new ParserError(message, level);
        }
    }

    public enum ParseErrorLevel
    {
        Warning,
        Error
    }

}
