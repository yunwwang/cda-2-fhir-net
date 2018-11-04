using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Extension;
using Wyw.Cda2Fhir.Core.Serialization.DataType;
using Wyw.Cda2Fhir.Core.Serialization.Resource;

namespace Wyw.Cda2Fhir.Core.Serialization
{
    public class SectionParser  : BaseParser<Composition.SectionComponent>
    {
        public SectionParser() : base()
        {
        }

        public SectionParser(Bundle bundle) : base(bundle)
        {
        }

        public override Composition.SectionComponent FromXml(XElement element)
        {
            if (element == null)
                return null;

            var section = new Composition.SectionComponent();

            foreach (var child in element.Elements())
                switch (child.Name.LocalName)
                {
                    case "code":
                        section.Code = FromXml(new CodeableConceptParser(), child);
                        break;

                    case "title":
                        section.Title = child.Value;
                        break;

                    case "text":
                        section.Text = new Narrative
                        {
                            Div = $"<div xmlns=\"http://www.w3.org/1999/xhtml\">{child.FirstNode.ToString(SaveOptions.DisableFormatting)}</div>",
                            Status = Narrative.NarrativeStatus.Generated
                        };
                        break;

                    case "entry":
                        AddEntryAct(section, child.CdaElement("act"));
                        break;
                }

            return section;
        }

        public void AddEntryAct(Composition.SectionComponent section, XElement element)
        {
            if (section == null || element == null)
                return;

            Hl7.Fhir.Model.Resource resource = null;
            switch (section.Code.Coding[0].Code)
            {
                case "48765-2":
                    resource = FromXml(new AllergyIntoleranceParser(Bundle), element);
                    break;
            }

            if (resource != null)
                section.Entry.Add(resource.GetResourceReference());
        }
    }
}
