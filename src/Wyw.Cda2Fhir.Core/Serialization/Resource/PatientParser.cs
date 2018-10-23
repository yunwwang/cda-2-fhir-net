using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Wyw.Cda2Fhir.Core.Extension;
using Wyw.Cda2Fhir.Core.Model;
using Wyw.Cda2Fhir.Core.Serialization.DataType;
using Wyw.Cda2Fhir.Core.Serialization.ValueSet;

namespace Wyw.Cda2Fhir.Core.Serialization.Resource
{
    public class PatientParser : BaseParser<Patient>
    {
        public PatientParser()
        {
        }

        public PatientParser(Bundle bundle) : base(bundle)
        {
        }

        public override Patient FromXml(XElement element)
        {
            if (element == null)
                return null;

            var patient = new Patient
            {
                Id = Guid.NewGuid().ToString(),
                Meta = new Meta
                {
                    Profile = new List<string>
                    {
                        "http://hl7.org/fhir/us/core/StructureDefinition/us-core-patient"
                    }
                }
            };

            Bundle?.AddResourceEntry(patient, null);

            foreach (var child in element.Elements())
                if (child.Name.LocalName == "id")
                {
                    var id = FromXml(new IdentifierParser(), child);

                    if (id != null)
                        patient.Identifier.Add(id);
                }
                else if (child.Name.LocalName == "addr")
                {
                    var addr = FromXml(new AddressParser(), child);

                    if (addr != null)
                        patient.Address.Add(addr);
                }
                else if (child.Name.LocalName == "telecom")
                {
                    var contactPoint = FromXml(new ContactPointParser(), child);

                    if (contactPoint != null)
                        patient.Telecom.Add(contactPoint);
                }
                else if (child.Name.LocalName == "patient")
                {
                    ParsePatient(patient, child);
                }
                else if (child.Name.LocalName == "providerOrganization")
                {
                    var org = FromXml(new OrganizationParser(Bundle), child);
                    if (org != null && Bundle != null)
                        patient.ManagingOrganization = new ResourceReference($"{org.TypeName}/{org.Id}");
                }

            if (!patient.Identifier.Any())
                Errors.Add(ParserError.CreateParseError(element, "does NOT have id element", ParseErrorLevel.Error));

            if (!patient.Name.Any())
                Errors.Add(ParserError.CreateParseError(element, "does NOT have name element", ParseErrorLevel.Error));

            if (patient.Gender == null)
                Errors.Add(ParserError.CreateParseError(element, "does NOT have administrativeGenderCode element",
                    ParseErrorLevel.Error));

            return patient;
        }

        private void ParsePatient(Patient patient, XElement element)
        {
            foreach (var child in element.Elements())
                if (child.Name.LocalName == "name")
                {
                    var name = FromXml(new HumanNameParser(), child);
                    if (name != null)
                        patient.Name.Add(name);
                }
                else if (child.Name.LocalName == "administrativeGenderCode")
                {
                    patient.Gender = new AdministrativeGenderParser().FromCda(child.Attribute("code")?.Value);
                }
                else if (child.Name.LocalName == "birthTime")
                {
                    patient.BirthDateElement = FromXml(new DateParser(), child);
                }
                else if (child.Name.LocalName == "maritalStatusCode")
                {
                    patient.MaritalStatus = FromXml(new CodeableConceptParser(), child);
                }
                else if (child.Name.LocalName == "religiousAffiliationCode")
                {
                    var religion = FromXml(new CodeableConceptParser(), child);
                    if (religion != null)
                        patient.AddExtension("http://hl7.org/fhir/StructureDefinition/patient-religion", religion);
                }
                else if (child.Name.LocalName == "raceCode")
                {
                    AddRaceCode(patient, child);
                }
                else if (child.Name.LocalName == "ethnicGroupCode")
                {
                    AddEthnicGroupCode(patient, child);
                }
                else if (child.Name.LocalName == "guardian")
                {
                    var relatedPerson = FromXml(new RelatedPersonParser(), child);
                    if (relatedPerson != null && Bundle != null)
                    {
                        relatedPerson.Patient = new ResourceReference($"{patient.TypeName}/{patient.Id}");
                        Bundle?.AddResourceEntry(relatedPerson, null);
                    }
                }
                else if (child.Name.LocalName == "birthplace")
                {
                    var address = FromXml(new AddressParser(), child.CdaElement("place")?.CdaElement("addr"));
                    if (address != null)
                        patient.AddExtension("http://hl7.org/fhir/StructureDefinition/birthPlace", address);
                }
                else if (child.Name.LocalName == "languageCommunication")
                {
                    AddLanguageCommunication(patient, child);
                }
        }

        private void AddRaceCode(Patient patient, XElement element)
        {
            const string url = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-race";

            var race = FromXml(new CodingParser(), element);

            if (race == null) return;

            // Remove display text
            var display = race.Display;

            race.Display = null;

            var raceExtension = patient.Extension.FirstOrDefault(e => e.Url == url);


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
                    raceExtension.AddExtension("ombCategory", race);
                    break;

                default:
                    raceExtension.AddExtension("detailed", race);
                    break;
            }

            if (raceExtension.Extension.All(e => e.Url != "text"))
                raceExtension.AddExtension("text", new FhirString(display));
        }

        private void AddEthnicGroupCode(Patient patient, XElement element)
        {
            const string url = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-ethnicity";

            var ethnicity = FromXml(new CodingParser(), element);

            if (ethnicity == null) return;

            var display = ethnicity.Display;

            ethnicity.Display = null;

            var ethnicityExtension = patient.Extension.FirstOrDefault(e => e.Url == url);


            if (ethnicityExtension == null)
            {
                ethnicityExtension = new Hl7.Fhir.Model.Extension {Url = url};
                patient.Extension.Add(ethnicityExtension);
            }

            switch (ethnicity.Code)
            {
                case "2135-2":
                case "2186-5":
                    ethnicityExtension.AddExtension("ombCategory", ethnicity);
                    break;

                default:
                    ethnicityExtension.AddExtension("detail", ethnicity);
                    break;
            }

            if (ethnicityExtension.Extension.All(e => e.Url != "text"))
                ethnicityExtension.AddExtension("text", new FhirString(display));
        }

        private void AddLanguageCommunication(Patient patient, XElement element)
        {
            var communication = new Patient.CommunicationComponent();

            foreach (var child in element.Elements())
                switch (child.Name.LocalName)
                {
                    case "languageCode":
                        var code = FromXml(new CodeParser(), child);
                        if (code != null)
                            communication.Language = new CodeableConcept("urn:ietf:bcp:47", code.Value);
                        break;

                    case "preferenceInd":
                        communication.PreferredElement = new FhirBooleanParser().FromXml(child, Errors);
                        break;
                }

            if (communication.Language != null)
                patient.Communication.Add(communication);
        }
    }
}