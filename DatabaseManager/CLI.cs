
using System;
using System.Threading.Tasks;
using Common.DatabaseManagerCommon;

namespace DatabaseManger
{
    public class CLI
    {
        public string token = "asddasasd";
        public async void Start()
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
                else if (option == 1) await AddTagHandler();
                else if (option == 2) await RemoveTagHandler();
                else if (option == 3) await ChangeTagScanningHandler();
                else if (option == 4) await WriteTagHandler();
                else if (option == 5) await ShowCurrentTagValuesHandler();
                else if (option == 6) await RegisterHandler();
                else if (option == 7) await LoginHandler();
                else await LogoutHandler();
            }
        }

        public void Menu()
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("(0) Exit");
            if (this.token != "") {
                Console.WriteLine("(1) Add Tag");
                Console.WriteLine("(2) Remove Tag");
                Console.WriteLine("(3) Change Tag Scanning");
                Console.WriteLine("(4) Write Tag Output Value");
                Console.WriteLine("(5) Show Current Tag Values");
            }
            Console.WriteLine("(6) Register");
            Console.WriteLine("(7) Login");
                
            if (this.token != "") Console.WriteLine("(8) Logout");

            Console.WriteLine("---------------------------------------");
        }

        public async Task AddTagHandler() { }

        public async Task RemoveTagHandler()
        {
            DeleteTagDTO dto = new DeleteTagDTO();
            dto.token = token;
            Console.Write("Enter name of the tag to be deleted: ");
            dto.TagName = Console.ReadLine();
            await HttpManager.DeleteTag(dto);
        }
        public async Task ChangeTagScanningHandler() { }
        public async Task WriteTagHandler() { }
        public async Task ShowCurrentTagValuesHandler() { }
        public async Task RegisterHandler() { }
        public async Task LoginHandler() { }
        public async Task LogoutHandler() { }
    }
}