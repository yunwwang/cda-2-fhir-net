using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Model;

namespace Wyw.Cda2Fhir.Core.Serialization
{
    public abstract class BaseParser
    {
        
        public ParseResult Result { get; set; } = new ParseResult();
        public List<ParseError> ParseErrors { get; set; } = new List<ParseError>();


    }
}
