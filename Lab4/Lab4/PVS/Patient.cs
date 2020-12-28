using System;
using System.Collections.Generic;

namespace Lab4.PVS
{
    class Patient
    {
        public String PatientNumber { get; set; }
        public String PatientName { get; set; }
        public String DOB { get; set; }
        public List<String> PatientsVaccinesList;

        public string GetVaccinesTakenAsString()
        {
            string vaccinesTaken = "";
            foreach (var vaccine in PatientsVaccinesList)
                vaccinesTaken += vaccine + " ";
            return vaccinesTaken;
        }
        public void Print()
        {
            Console.WriteLine(" PatientNumber : {0}", PatientNumber);
            Console.WriteLine(" PatientName : {0}", PatientName);
            Console.WriteLine(" DOB : {0}", DOB);
            Console.WriteLine("PatientsVaccinesList");

            foreach (String s in PatientsVaccinesList)
                Console.Write(s + " ");

            Console.WriteLine();
            Console.WriteLine("*********************************");
        }

        public Patient() 
        {
            PatientsVaccinesList = new List<String>();
        }
        public Patient(String number, String name, String DOB, List<String> VacList)
        {
            this.PatientNumber = number;
            this.PatientName = name;
            this.DOB = DOB;
            this.PatientsVaccinesList = VacList;
        }
    }
}
