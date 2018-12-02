using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wyw.Cda2Fhir.Core.Serialization.ValueSet
{
    public class AllergyIntoleranceSeverityParser
    {
        public AllergyIntolerance.AllergyIntoleranceSeverity? FromCda(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            switch (value)
            {
                case "24484000":
                    return AllergyIntolerance.AllergyIntoleranceSeverity.Severe;
                case "255604002":
                    return AllergyIntolerance.AllergyIntoleranceSeverity.Mild;
                case "6736007":
                    return AllergyIntolerance.AllergyIntoleranceSeverity.Moderate;
                default:
                    return null;
            }
        }
    }
}
