using System;
using System.Collections.Generic;
using System.Text;

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

    }

    public enum ParseErrorLevel
    {
        Warning,
        Error
    }

}
