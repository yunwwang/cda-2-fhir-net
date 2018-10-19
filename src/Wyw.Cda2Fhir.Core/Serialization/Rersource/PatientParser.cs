using System;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Serialization.DataType;
using Wyw.Cda2Fhir.Core.Serialization.ValueSet;

namespace Wyw.Cda2Fhir.Core.Serialization
{
    public class PatientParser : BaseParser
    {
        public Patient FromXml(XElement element)
        {
            if (element == null)
                return null;

            var patient = new Patient()
            {
                Id = Guid.NewGuid().ToString()
            };

            foreach (var child in element.Elements())
            {
                switch (child.Name.LocalName)
                {
                    case "id":
                        var id = new IdentifierParser().FromXml(child);
                        if (id != null)
                            patient.Identifier.Add(id);
                        break;
                    case "addr":
                        var addr = new AddressParser().FromXml(child);
                        if (addr != null)
                            patient.Address.Add(addr);
                        break;
                    case "telecom":
                        var contactPoint = new ContactPointParser().FromXml(child);
                        if (contactPoint != null)
                            patient.Telecom.Add(contactPoint);
                        break;
                    case "patient":
                        ParsePatient(patient, child);
                        break;
                }
            }

            return patient;
        }

        private void ParsePatient(Patient patient, XElement element)
        {
            var raceExtension = new Hl7.Fhir.Model.Extension
            {
                Url = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-race"
            };

            var ethnicityExtension = new Hl7.Fhir.Model.Extension
            {
                Url = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-ethnicity"
            };

            foreach(var child in element.Elements())
            switch (child.Name.LocalName)
            {
                    case "name":
                        var name = new HumanNameParser().FromXml(child);
                        if (name != null)
                            patient.Name.Add(name);
                        break;

                    case "administrativeGenderCode":
                        patient.Gender = new AdministrativeGenderParser().FromCda(child.Attribute("code")?.Value);
                        break;

                    case "birthTime":
                        patient.BirthDateElement = new DateParser().FromXml(child);
                        break;

                    case "maritalStatusCode":
                        patient.MaritalStatus = new CodeableConceptParser().FromXml(child);
                        break;

                    case "religiousAffiliationCode":
                        var religion = new CodeableConceptParser().FromXml(child);
                        if (religion != null)
                            patient.Extension.Add(new Hl7.Fhir.Model.Extension
                            {
                                Url = "http://hl7.org/fhir/StructureDefinition/patient-religion",
                                Value = religion
                            });
                        break;
            }
        }
    }
}
