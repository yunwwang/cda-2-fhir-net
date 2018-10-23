using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Extension;
using Wyw.Cda2Fhir.Core.Serialization.DataType;

namespace Wyw.Cda2Fhir.Core.Serialization.Resource
{
    public class DeviceParser : BaseParser<Device>
    {
        public DeviceParser()
        {
        }

        public DeviceParser(Bundle bundle) : base(bundle)
        {
        }

        public override Device FromXml(XElement element)
        {
            if (element == null)
                return null;

            var device = new Device()
            {
                Id = Guid.NewGuid().ToString()
            };

            var location = new Location()
            {
                Id = Guid.NewGuid().ToString()
            };

            Bundle?.Entry.Add(new Bundle.EntryComponent { Resource = device });

            foreach (var child in element.Elements())
            {
                if (child.Name.LocalName == "id")
                {
                    var id = FromXml(new IdentifierParser(), child);

                    if (id != null)
                        device.Identifier.Add(id);
                }
                else if (child.Name.LocalName == "code")
                {
                    device.Type = FromXml(new CodeableConceptParser(), child);
                }
                else if (child.Name.LocalName == "addr")
                {
                    location.Address = FromXml(new AddressParser(), child);
                }
                else if (child.Name.LocalName == "telecom")
                {
                    var telecom = FromXml(new ContactPointParser(), child);
                    if (telecom != null)
                        location.Telecom.Add(telecom);
                }
                else if (child.Name.LocalName == "assignedAuthoringDevice")
                {
                    device.Model = child.CdaElement("manufacturerModelName")?.Value;
                }
            }

            if (location.Address != null || location.Telecom.Any())
            {
                device.Location = new ResourceReference($"{location.TypeName}/{location.Id}");
                Bundle?.Entry.Add(new Bundle.EntryComponent(){Resource = location});
            }

            return device;
        }

    }
}
