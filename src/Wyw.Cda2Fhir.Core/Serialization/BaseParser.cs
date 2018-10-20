using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Model;

namespace Wyw.Cda2Fhir.Core.Serialization
{
    public interface IParser<T> where T: Base
    {
        List<ParseError> Errors { get; set; }
        T FromXml(XElement element);
    }

    public abstract class BaseParser<T> :IParser<T>  where T: Base
    {
        public List<ParseError> Errors { get; set; } = new List<ParseError>();

        public abstract T FromXml(XElement element);
    }
}
