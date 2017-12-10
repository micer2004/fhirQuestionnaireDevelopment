using System;
using System.Collections.Generic;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace HL7.FHIR.Core.Questionaire
{
    static class Program
    {
        static void Main()
        {

            var qDødsmelding = new Questionnaire
            {
                Id = "7",
                Title = "Dødsmelding",
                Name = "doedsmelding_nb_01",
                Description = new Markdown("Melding som rapporterer dødsfall til Folkeregisteret"),
                Purpose = new Markdown("Melding som skal fylles ut av lege ved dødsfall"),
                Publisher = "Direktoratet for e-helse",
                Date = "2017",
                Status = PublicationStatus.Draft,
                Version = "1",
                Url = "http://nde-fhir-ehelse.azurewebsites.net/fhir/Questionnaire/7",
                Language = "nb-NO",
                Meta = new Meta()
                {
                  Tag  = new List<Coding>()
                  {
                      new Coding()
                      {
                          System = "https://sarepta.ehelse.no/FHIR/Questionnaire_doedsmelding",
                          Version = "1.0",
                      },
                      new Coding()
                      {
                          System = "urn:ietf:bcp:47",
                          Code = "nb-NO",
                          Display = "Norsk (bokmål)"
                      }
                  },
                  Profile = new List<string>()
                  {
                      "https://sarepta.ehelse.no/FHIR/StructureDefinition/SDF-Questionnaire"
                  }
                },
                SubjectType = new List<ResourceType?>()
                {
                    ResourceType.Person
                },
                Item = new List<Questionnaire.ItemComponent>()
                {
                    new Questionnaire.ItemComponent()
                    {
                        LinkId = "deceasedPerson",
                        Text = "Personidentifikator til den avdøde",
                        Required = true,
                        Repeats = false,
                        Type = Questionnaire.QuestionnaireItemType.Text,
                        FhirComments = new List<string>() {"PersonPicker"},
                        
                    },
                    new Questionnaire.ItemComponent()
                    {
                        LinkId = "timeOfDeath",
                        Text = "Dødstidspunkt (dd.mm.åååå)",
                        Required = true,
                        Repeats = false,
                        Type = Questionnaire.QuestionnaireItemType.DateTime
                    },
                    new Questionnaire.ItemComponent()
                    {
                        LinkId = "confirmation",
                        EnableWhen = new List<Questionnaire.EnableWhenComponent>()
                        {
                            new Questionnaire.EnableWhenComponent()
                            {
                                Question = "timeOfDeath",
                                HasAnswer = true
                            }
                        },
                        Text = "Bekreftelse",
                        Required = true,
                        Repeats = false,
                        Type = Questionnaire.QuestionnaireItemType.Boolean
                    }
                }
            };


            const string fhirServer = "http://nde-fhir-ehelse.azurewebsites.net/fhir";
            //const string fhirServer = "http://vonk.furore.com";
            var client = new FhirClient(fhirServer);

            try
            {
                //var resCreate = client.Create(qDødsmelding);

                var resCreate = client.Update(qDødsmelding);
                Console.WriteLine($"Questionnaire ID: {resCreate.Id}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Finished...");
        }
    }
}
