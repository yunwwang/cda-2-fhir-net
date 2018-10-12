using System;
using System.Collections.Generic;
using System.Text;

namespace Wyw.Cda2Fhir.Core.Model
{
    public class Constant
    {
        public static readonly List<KnownCodeSystem> KnownCodeSystemList = new List<KnownCodeSystem>()
        {
            // LOINC
            new KnownCodeSystem
            {
                Uri = "http://loinc.org",
                Oid = "2.16.840.1.113883.6.1"
            },

            // SNOMED CT
            new KnownCodeSystem
            {
                Uri = "http://snomed.info/sct",
                Oid = "2.16.840.1.113883.6.96"
            },
        };
    }

    public class KnownCodeSystem
    {
        public string Uri { get; set; }
        public string Oid { get; set; }

    }
}
