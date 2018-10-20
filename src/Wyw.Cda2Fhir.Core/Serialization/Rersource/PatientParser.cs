using System;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Extension;
using Wyw.Cda2Fhir.Core.Model;
using Wyw.Cda2Fhir.Core.Serialization.DataType;
using Wyw.Cda2Fhir.Core.Serialization.Rersource;
using Wyw.Cda2Fhir.Core.Serialization.ValueSet;

namespace Wyw.Cda2Fhir.Core.Serialization
{
    public class PatientParser : BaseParser<Patient>
    {
        public PatientParser()
        {
        }

        public PatientParser(Bundle bundle)
        {
            Bundle = bundle;
        }

        private Bundle Bundle { get; }

        public override Patient FromXml(XElement element)
        {
            if (element == null)
                return null;

            var patient = new Patient
            {
                Id = Guid.NewGuid().ToString()
            };

            foreach (var child in element.Elements())
                if (child.Name.LocalName == "id")
                {
                    var parser = new IdentifierParser();
                    var id = parser.FromXml(child);

                    if (id == null) continue;

                    patient.Identifier.Add(id);
                    Errors.AddRange(parser.Errors);
                }
                else if (child.Name.LocalName == "addr")
                {
                    var parser = new AddressParser();
                    var addr = parser.FromXml(child);

                    if (addr == null) continue;

                    patient.Address.Add(addr);
                    Errors.AddRange(parser.Errors);
                }
                else if (child.Name.LocalName == "telecom")
                {
                    var parser = new ContactPointParser();
                    var contactPoint = parser.FromXml(child);

                    if (contactPoint == null)
                        continue;

                    patient.Telecom.Add(contactPoint);
                    Errors.AddRange(parser.Errors);
                }
                else if (child.Name.LocalName == "patient")
                {
                    ParsePatient(patient, child);
                }

            return patient;
        }

        private void ParsePatient(Patient patient, XElement element)
        {
            foreach (var child in element.Elements())
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

                    case "raceCode":
                        AddRaceCode(patient, child);
                        break;

                    case "ethnicGroupCode":
                        AddEthnicGroupCode(patient, child);
                        break;

                    case "guardian":
                        var relatedPerson = new RelatedPersonParser().FromXml(child);
                        if (relatedPerson != null && Bundle != null)
                        {
                            relatedPerson.Patient = new ResourceReference("Patient/" + patient.Id);
                            Bundle.Entry.Add(new Bundle.EntryComponent {Resource = relatedPerson});
                        }

                        break;

                    case "birthplace":
                        var address = new AddressParser().FromXml(child.CdaElement("place")?.CdaElement("addr"));
                        if (address != null)
                            patient.Extension.Add(
                                new Hl7.Fhir.Model.Extension("http://hl7.org/fhir/StructureDefinition/birthPlace",
                                    address));
                        break;

                    case "languageCommunication":
                        AddLanguageCommunication(patient, child);
                        break;
                }
        }

        private void AddRaceCode(Patient patient, XElement element)
        {
            const string url = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-race";

            var race = new CodingParser().FromXml(element);

            var raceExtension = patient.Extension.FirstOrDefault(e => e.Url == url);

            if (race != null)
            {
                if (raceExtension == null)
                {
                    raceExtension = new Hl7.Fhir.Model.Extension {Url = url};
                    patient.Extension.Add(raceExtension);
                }

                switch (race.Code)
                {
                    case "1002-5":
                    case "2028-9":
                    case "2054-5":
                    case "2076-8":
                    case "2106-3":
                    case "UNK":
                    case "ASKU":
                        raceExtension.Extension.Add(new Hl7.Fhir.Model.Extension("ombCategory", race));
                        break;

                    default:
                        raceExtension.Extension.Add(new Hl7.Fhir.Model.Extension("detailed", race));
                        break;
                }
            }
        }

        private void AddEthnicGroupCode(Patient patient, XElement element)
        {
            const string url = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-ethnicity";

            var ethnicity = new CodingParser().FromXml(element);

            var ethnicityExtension = patient.Extension.FirstOrDefault(e => e.Url == url);


            if (ethnicity != null)
            {
                if (ethnicityExtension == null)
                {
                    ethnicityExtension = new Hl7.Fhir.Model.Extension {Url = url};
                    patient.Extension.Add(ethnicityExtension);
                }

                switch (ethnicity.Code)
                {
                    case "2135-2":
                    case "2186-5":
                        ethnicityExtension.Extension.Add(
                            new Hl7.Fhir.Model.Extension("ombCategory", ethnicity));
                        break;

                    default:
                        ethnicityExtension.Extension.Add(new Hl7.Fhir.Model.Extension("detail", ethnicity));
                        break;
                }
            }
        }

        private void AddLanguageCommunication(Patient patient, XElement element)
        {
            var communication = new Patient.CommunicationComponent();

            foreach (var child in element.Elements())
            {
                switch (child.Name.LocalName)
                {
                    case "languageCode":
                        var code = new CodeParser().FromXml(child);
                        if (code != null)
                            communication.Language = new CodeableConcept("urn:ietf:bcp:47", code.Value);
                        break;

                    case "preferenceInd":
                        communication.PreferredElement = new FhirBooleanParser().FromXml(child);
                        break;
                }
            }

            if (communication.Language != null)
                patient.Communication.Add(communication);
        }
    }
}