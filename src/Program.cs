﻿using System;
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
            Console.WriteLine($"Total of Patients:{patientsBundle.Total}");

            foreach (Bundle.EntryComponent entry in patientsBundle.Entry)
            {

                if (entry != null)
                {
                    Patient patient = (Patient)entry.Resource;
                    Console.WriteLine($"The Patient name is {patient.Name}");
                }
            }
        }
    }
}
