using System;

namespace Lab4.PVS
{
    class Menu
    {
        private ManageSystem pvs;
        public void Run()
        {
            int MenuOption = -1;
            while (MenuOption != 0)
            {
                Display();
                MenuOption = Convert.ToInt16(Console.ReadLine());
                Process(MenuOption);
                if (MenuOption != 0)
                    Console.WriteLine("Press enter to continue ...");
                else
                    Console.WriteLine("Application terminated.");
                Console.ReadLine();
                Console.Clear();
            }
        }
        private void Display()
        {
            Console.WriteLine("**** Menu ******");
            Console.WriteLine("1 - Set up patient data.");
            Console.WriteLine("2 - Display patint data.");
            Console.WriteLine("3 - Add a new vaccine to the system.");
            Console.WriteLine("4 - Add a vaccine to patient information.");
            Console.WriteLine("5 - percentage of patients that have taken each vaccine.");
            Console.WriteLine("6 - list of patients per vac.");
            Console.WriteLine("7 - Dynamic query both.");
            Console.WriteLine("8 - Dynamic query either.");
            Console.WriteLine("9 - Save.");
            Console.WriteLine("0 - Exit.");
            Console.Write("Enter option: ");
        }
        private void Process(int MenuOption)
        {
            switch (MenuOption)
            {
                case (1):
                    pvs.setupTestData();
                    break;

                case (2):
                    pvs.PrintPatientInfo();
                    break;

                case (3):
                    pvs.AddVaccine();
                    break;

                case (4):
                    pvs.AddVaccineToPatient();
                    break;

                case (5):
                    pvs.DisplayPercent();
                    break;

                case (6):
                    pvs.DisplayVaccinePatient();
                    break;

                case (7):
                    pvs.QueryBoth();
                    break;

                case (8):
                    pvs.QueryEither();
                    break;

                case (9):
                    pvs.SaveOut();
                    break;

                default:
                    break;
            }
        }
        public Menu() 
        {
            pvs = new ManageSystem();
        }
    }
}
