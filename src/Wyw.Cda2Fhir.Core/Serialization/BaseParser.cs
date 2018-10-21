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
        List<ParserError> Errors { get; set; }
        T FromXml(XElement element);
        T FromXml(XElement element, List<ParserError> errors);
    }

    public abstract class BaseParser<T> :IParser<T>  where T: Base
    {
        public List<ParserError> Errors { get; set; } = new List<ParserError>();

        public abstract T FromXml(XElement element);

        public virtual T FromXml(XElement element, List<ParserError> errors)
        {
            var result = FromXml(element);

            errors?.AddRange(Errors);

            return result;
        }
    }
}
