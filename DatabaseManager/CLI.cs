
using System;

namespace DatabaseManger
{
    public class CLI
    {
        public static void Start()
        {
            while (true)
            {
                Menu();
                int option;
                while (true) {
                    Console.Write("> ");
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out option) && option >= 0 && option <= 8) break;
                    Console.WriteLine("Malformed input, try again!");
                }

                if (option == 0) break;
                else if (option == 1) AddTagHandler();
                else if (option == 2) RemoveTagHandler();
                else if (option == 3) ChangeTagScanningHandler();
                else if (option == 4) WriteTagHandler();
                else if (option == 5) ShowCurrentTagValuesHandler();
                else if (option == 6) RegisterHandler();
                else if (option == 7) LoginHandler();
                else LogoutHandler();
            }
        }

        public static void Menu()
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("(0) Exit");
            Console.WriteLine("(1) Add Tag");
            Console.WriteLine("(2) Remove Tag");
            Console.WriteLine("(3) Change Tag Scanning");
            Console.WriteLine("(4) Write Tag Output Value");
            Console.WriteLine("(5) Show Current Tag Values");
            Console.WriteLine("(6) Register");
            Console.WriteLine("(7) Login");
            Console.WriteLine("(8) Logout");
            Console.WriteLine("---------------------------------------");
        }

        public static void AddTagHandler() { }
        public static void RemoveTagHandler() { }
        public static void ChangeTagScanningHandler() { }
        public static void WriteTagHandler() { }
        public static void ShowCurrentTagValuesHandler() { }
        public static void RegisterHandler() { }
        public static void LoginHandler() { }
        public static void LogoutHandler() { }
    }
}