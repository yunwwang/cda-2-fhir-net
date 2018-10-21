using System;
using System.Collections.Generic;
using System.Text;
using Hl7.Fhir.Model;

namespace Wyw.Cda2Fhir.Core.Model
{
    public class ParseResult
    {
        public Base Resource { get; set; }
        public List<ParserError> Errors { get; set; } = new List<ParserError>();

        public ParseResult()
        {
        }

        public ParseResult(Base resource, List<ParserError> errors)
        {
            Resource = resource;
            Errors = errors;
        }
    }
}
