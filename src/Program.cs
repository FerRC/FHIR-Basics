using System;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;

namespace FHIR_Basics
{
    public static class Program
    {
        private const string server = "http://server.fire.ly";
        static void Main(string[] args)
        {
            var settings = new FhirClientSettings
            {
                Timeout = 1200,
                PreferredFormat = ResourceFormat.Json,
                VerifyFhirVersion = true,
                PreferredReturn = Prefer.ReturnMinimal
            };

            FhirClient client = new FhirClient(server, settings);

            Bundle patientsBundle = client.Search<Patient>(null);
            Console.WriteLine($"Total of Patients:{patientsBundle.Total} and the Entry Count is:{patientsBundle.Entry.Count}");

            foreach (Bundle.EntryComponent patientEntry in patientsBundle.Entry)
            {

                if (patientEntry.Resource != null)
                {
                    Patient patient = (Patient)patientEntry.Resource;
                    if (patient.Name.Count > 0)
                    {
                        Console.WriteLine($"The Patient name is {patient.Name[0]}");
                    }

                    Bundle encounterBundle = client.Search<Encounter>(
                        new string[]
                        {
                            $"patient=Patient/{patient.Id}"
                        });

                    Console.WriteLine($"The total encounters are:{encounterBundle.Total} and the Entry count is:{encounterBundle.Entry.Count}");
                    foreach (Bundle.EntryComponent encounterEntry in encounterBundle.Entry)
                    {
                        if (encounterEntry.Resource != null)
                        {
                            Encounter encounter = (Encounter)encounterEntry.Resource;
                            Console.WriteLine($"\tThe Encounter number is:{encounter.Id} and the date is:{encounter.Period.Start}");
                        }
                    }
                }
            }
        }
    }
}
