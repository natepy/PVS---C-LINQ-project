using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lab4.PVS
{
    class ManageSystem
    {
        private List<Patient> patientsInSystem;
        private List<String> vaccinesList;

        private List<string> QueryList()
        {
            // get vaccine list
            int Option = -1;
            List<string> Vaccines = new List<string>();
            while (Option != 0)
            {
                Console.WriteLine();
                Console.WriteLine("Enter option:");
                Console.WriteLine("\t1 - Add to list.");
                Console.WriteLine("\t0 - Confirm list.");
                Console.Write("Enter option: ");
                Option = Convert.ToInt32(Console.ReadLine());
                if (Option == 1)
                {
                    Console.Write("Enter vaccine name: ");
                    Vaccines.Add(Console.ReadLine());
                }
            }
            return Vaccines;
        }
        public void QueryBoth()
        {
            // get vaccine list
            List<string> Vaccines = QueryList();
            // searching
            Console.WriteLine("\nThese patients have taken a vaccine from the given list.");
            foreach (var patient in patientsInSystem)
            {
                var PatientVaccineIntersect = patient.PatientsVaccinesList.Intersect(Vaccines).ToList();
                if (PatientVaccineIntersect.All(Vaccines.Contains) && PatientVaccineIntersect.Count == Vaccines.Count)
                    Console.WriteLine(patient.PatientNumber);
            }
        }
        public void QueryEither()
        {
            // track
            bool doesContain = false;
            // get vaccine list
            List<string> Vaccines = QueryList();
            // searching
            Console.WriteLine("\nThese patients have taken a vaccine from the given list.");
            foreach (var patient in patientsInSystem)
            {
                doesContain = patient.PatientsVaccinesList.Intersect(Vaccines).Any();
                if (doesContain)
                    Console.WriteLine(patient.PatientNumber);
            }
        }
        public void PrintPatientInfo()
        {
            patientsInSystem.ForEach(p => p.Print());
            Console.WriteLine("\nTotal patients: {0}", patientsInSystem.Count());
        }
        public void DisplayPercent()
        {
            var PairInfo = patientsInSystem.SelectMany(p => p.PatientsVaccinesList.Select(v => v))
                                           .GroupBy(s => s)
                                           .Select(s => new { Vaccine = s.Key, Percent = (s.Count() * 100) / patientsInSystem.Count });
            foreach (var pvsGroup in PairInfo)
                Console.WriteLine("{0}*{1}%", pvsGroup.Vaccine, pvsGroup.Percent);
            Console.WriteLine("\nTotal vaccines: {0}", vaccinesList.Count());
        }
        public void DisplayVaccinePatient()
        {
            var PairInfo = patientsInSystem.SelectMany(p => p.PatientsVaccinesList, (p, v) => new { vaccine = v, Number = p.PatientNumber })
                                           .OrderBy(v => v.vaccine)
                                           .GroupBy(v => v.vaccine, v => new { PatientList = v.Number })
                                           .ToList();
            foreach (var pvsGroup in PairInfo)
            {
                Console.WriteLine("{0} Size {1}", pvsGroup.Key, pvsGroup.Count());
                foreach (var item in pvsGroup)
                    Console.WriteLine(item.PatientList);
            }
        }
        public void AddVaccineToPatient()
        {
            // Get information on patient and vaccine
            string VaccineName, PatientNumber;
            bool patientExists;
            Console.Write("Enter vaccine name: ");
            VaccineName = Console.ReadLine();
            Console.Write("Enter patient number: ");
            PatientNumber = Console.ReadLine();
            // Validate information
            patientExists = patientsInSystem.Any(p => p.PatientNumber == PatientNumber);
            if (vaccinesList.Contains(VaccineName))
            {
                if (patientExists)
                    AddToPatient(PatientNumber, VaccineName);
                else
                    Console.WriteLine("Patient does not exist.");
            }
            else
                Console.WriteLine("Vaccine does not exist in our system.");
        }
        private void AddToPatient(string PatientNr, string Vaccine)
        {
            foreach (var patient in patientsInSystem)
                if (patient.PatientNumber == PatientNr)
                    patient.PatientsVaccinesList.Add(Vaccine);
        }
        public void AddVaccine()
        {
            string NewVaccine;
            Console.Write("Enter new vaccine name: ");
            NewVaccine = Console.ReadLine();
            if (vaccinesList.Contains(NewVaccine))
                Console.WriteLine("Vaccine {0} already exists in this system.", NewVaccine);
            else
            {
                vaccinesList.Add(NewVaccine);
                Console.WriteLine("Vaccine {0} has been successfully added to the system.", NewVaccine);
            }
        }
        private void SaveVaccines(string file)
        {
            FileInfo VaccineFile = new FileInfo(file);
            FileStream VaccineFileStream;
            BinaryFormatter FormatVaccineFile = new BinaryFormatter();

            if (VaccineFile.Exists)
                VaccineFileStream = new FileStream(file, FileMode.Truncate, FileAccess.Write);
            else
                VaccineFileStream = new FileStream(file, FileMode.Create, FileAccess.Write);

            try
            {
                foreach (var vaccine in vaccinesList)
                {
                    FormatVaccineFile.Serialize(
                        VaccineFileStream,
                        $"{vaccine}\n"
                    );
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
            VaccineFileStream.Close();
            Console.WriteLine("Vaccines saved to file {0}", VaccineFile.FullName);
        }
        private void SavePatients(string file)
        {
            FileInfo PatientFile = new FileInfo(file);
            FileStream PatientFileStream;
            BinaryFormatter FormatPatientFile = new BinaryFormatter();

            if (PatientFile.Exists)
                PatientFileStream = new FileStream(file, FileMode.Truncate, FileAccess.Write);
            else
                PatientFileStream = new FileStream(file, FileMode.Create, FileAccess.Write);

            try
            {
                foreach (var patient in patientsInSystem)
                {
                    FormatPatientFile.Serialize(
                        PatientFileStream,
                        $"{patient.PatientNumber}, {patient.PatientName}, {patient.DOB}, vaccines: {patient.GetVaccinesTakenAsString()}\n"
                    );
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
            PatientFileStream.Close();
            Console.WriteLine("Patients saved to file {0}", PatientFile.FullName);
        }
        public void SaveOut()
        {
            SavePatients("PatientsInfo.dat");
            SaveVaccines("VaccinesInfo.dat");
        }
        public void setupTestData()
        {
            vaccinesList = new List<string> { "covid19", "sars", "swineflu", "measles", "chickenpox", "mumps", "whooping cough", "polio", "memB", "memC", "rotavirus", "tetanus" };
            patientsInSystem = new List<Patient>()
            {
                 new Patient() { PatientNumber = "001", PatientName  = "Nathan O'Connor", DOB = "28/02/1999",PatientsVaccinesList = { "covid19", "sars", "swineflu"} },
                 new Patient() { PatientNumber = "002", PatientName  = "Oleg Vladimirovich", DOB = "23/03/1983",PatientsVaccinesList = {"measles"}},
                 new Patient() { PatientNumber = "003", PatientName  = "Keith Kelly", DOB = "09/05/2001",PatientsVaccinesList = {"memB"}},
                 new Patient() { PatientNumber = "004", PatientName  = "Micheal D. Higgens", DOB = "18/04/1941",PatientsVaccinesList = {"covid19"}},
                 new Patient() { PatientNumber = "005", PatientName  = "Mary McAleese", DOB = "27/06/1951",PatientsVaccinesList = {"memC"}},
                 new Patient() { PatientNumber = "006", PatientName  = "Patrick Hillery", DOB = "02/05/1923",PatientsVaccinesList = { "covid19"} },
                 new Patient() { PatientNumber = "007", PatientName  = "Cearbhall Ó Dálaigh", DOB = "12/02/1911",PatientsVaccinesList = {"covid19"}},
                 new Patient() { PatientNumber = "008", PatientName  = "Erskine H. Childers", DOB = "11/12/1905",PatientsVaccinesList = {"covid19","sars"}},
                 new Patient() { PatientNumber = "009", PatientName  = "Éamon de Velera", DOB = "14/10/1882",PatientsVaccinesList = {"rotavirus"}},
                 new Patient() { PatientNumber = "010", PatientName  = "Seán T. O'Kelly", DOB = "25/08/1882",PatientsVaccinesList = new List<string>()},
                 new Patient() { PatientNumber = "011", PatientName  = "Douglas Hyde", DOB = "17/01/1945",PatientsVaccinesList = new List<string>()},
                 new Patient() { PatientNumber = "012", PatientName  = "Tony Hawk", DOB = "12/05/1968",PatientsVaccinesList = {"covid19"}},
                 new Patient() { PatientNumber = "013", PatientName  = "John Cena", DOB = "23/04/1977",PatientsVaccinesList = {"sars", "swineflu"}},
                 new Patient() { PatientNumber = "014", PatientName  = "Mary Kelly", DOB = "09/11/1888",PatientsVaccinesList = {"sars", "swineflu", "mumps", "chickenpox", "whooping cough", "measles", "memB", "memC", "covid19", "polio", "rotavirus"}},
                 new Patient() { PatientNumber = "015", PatientName  = "Johnny Nitro", DOB = "03/10/1979",PatientsVaccinesList = {"covid19"}},
                 new Patient() { PatientNumber = "016", PatientName  = "Rey Mysterio", DOB = "11/12/1974",PatientsVaccinesList = { "covid19"} },
                 new Patient() { PatientNumber = "017", PatientName  = "Peter Parker", DOB = "10/08/2001",PatientsVaccinesList = {"polio"}},
                 new Patient() { PatientNumber = "018", PatientName  = "Mike Murphy", DOB = "20/10/1941",PatientsVaccinesList = {"whooping cough"}},
                 new Patient() { PatientNumber = "019", PatientName  = "Pádrig J. O'Leprosy", DOB = "01/01/1941",PatientsVaccinesList = {"mumps"}},
                 new Patient() { PatientNumber = "020", PatientName = "Rodraig S. O'Leprosy", DOB="01/01/1941", PatientsVaccinesList = {"mumps"} }
            };
            Console.WriteLine("Sample data set up.");
        }

        public ManageSystem()
        {
            patientsInSystem = new List<Patient>();
            vaccinesList = new List<string>();
        }
    }
}
