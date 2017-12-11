using System;
using System.Collections.Generic;
using System.Globalization;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace HL7.FHIR.Code.QuestionnaireResponse
{
    static class Program
    {
        static void Main()
        {

            var gPerson = Guid.NewGuid();
            var gHelseperson = Guid.NewGuid();

            var qResponse = new Hl7.Fhir.Model.QuestionnaireResponse
            {
                Id = "1",
                Meta = new Meta
                {
                    Tag = new List<Coding>()
                    {
                        new Coding()
                        {
                            System = "https://sarepta.ehelse.no/FHIR/QuestionnaireResponse_doedsmelding",
                            Version = "1.0",
                        },
                    },
                    Profile = new List<string>
                    {
                        "https://sarepta.ehelse.no/FHIR/StructureDefinition/SDF-QuestionnaireResponse_RapportDeceasedPerson"
                    }
                },
                Questionnaire = new ResourceReference()
                {
                    Url = new Uri("http://nde-fhir-ehelse.azurewebsites.net/fhir/Questionnaire/7")
                },
                Authored = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Status = Hl7.Fhir.Model.QuestionnaireResponse.QuestionnaireResponseStatus.Completed,
                Subject = new ResourceReference()
                {
                    Reference = $"https://grunndata.ehelse.no/fhir/Person/{gPerson}"  
                },
                Author = new ResourceReference
                {
                    Reference = $"https://grunndata.ehelse.no/fhir/Practitioner/{gHelseperson}"
                },
                Item = new List<Hl7.Fhir.Model.QuestionnaireResponse.ItemComponent>
                {
                    new Hl7.Fhir.Model.QuestionnaireResponse.ItemComponent
                    {
                      LinkId  = "deceasedPerson",
                      Answer = new List<Hl7.Fhir.Model.QuestionnaireResponse.AnswerComponent>
                      {
                          new Hl7.Fhir.Model.QuestionnaireResponse.AnswerComponent
                          {
                              //Value = new FhirUri("https://grunndata.ehelse.no/fhir/Person?identitifier=PID|12345678901")
                              Value = new FhirString("01041843345")
                          }
                      }
                    },
                    new Hl7.Fhir.Model.QuestionnaireResponse.ItemComponent
                    {
                        LinkId = "timeOfDeath",
                        Answer = new List<Hl7.Fhir.Model.QuestionnaireResponse.AnswerComponent>
                        {
                            new Hl7.Fhir.Model.QuestionnaireResponse.AnswerComponent
                            {
                                Value = new FhirDateTime(DateTime.Today.AddDays(-1))
                            }
                        } 
                        
                    },
                    new Hl7.Fhir.Model.QuestionnaireResponse.ItemComponent
                    {
                        LinkId = "confirmation",
                        Answer = new List<Hl7.Fhir.Model.QuestionnaireResponse.AnswerComponent>
                        {
                            new Hl7.Fhir.Model.QuestionnaireResponse.AnswerComponent
                            {
                                Value = new FhirBoolean(true)
                            }
                        }
                    }
                    
                }
            };

            const string fhirServer = "http://nde-fhir-ehelse.azurewebsites.net/fhir";

            var client = new FhirClient(fhirServer);

            try
            {
                var resCreate = client.Update(qResponse);
                Console.WriteLine($"QuestionnaireResponse ID: {resCreate.Id}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Finished...");
        }
    }
}
