namespace DatabaseManager;
using Common.DatabaseManagerCommon;

public class CLI
{
    public string token = "";
    public string username = "";
    public void Start()
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
        else
        {
            Console.WriteLine("(6) Register");
            Console.WriteLine("(7) Login");
        }

                
        if (this.token != "") Console.WriteLine("(8) Logout");

        Console.WriteLine("---------------------------------------");
    }

    // TODO
    public void AddTagHandler() { }

    public void RemoveTagHandler()
    {
        DeleteTagDTO dto = new DeleteTagDTO();
        dto.token = token;
        Console.Write("Enter name of the tag to be deleted: ");
        dto.TagName = Console.ReadLine().Trim();
        HttpManager.DeleteTag(dto).Wait();
    }

    public void ChangeTagScanningHandler()
    {
        ChangeScanTagDTO dto = new ChangeScanTagDTO();
        dto.token = token;
        Console.Write("Enter name of the tag to be deleted: ");
        dto.TagName = Console.ReadLine().Trim();

        while (true)
        {
            Console.Write("Tag scanning on (true/false):");
            string input = Console.ReadLine();
            bool state;
            if (!Boolean.TryParse(input, out state)) Console.WriteLine("Malformed input, try again!");
            else
            {
                dto.state = state;
                break;
            }
        }

        HttpManager.ChangeTagScanning(dto).Wait();
    }

    public void WriteTagHandler()
    {
        WriteTagValueDTO dto = new WriteTagValueDTO();
        dto.token = token;
        Console.Write("Enter name of the tag to be deleted: ");
        dto.TagName = Console.ReadLine().Trim();

        while (true)
        {
            Console.WriteLine("Enter the new value to be written: ");
            int value;
            string input = Console.ReadLine();
            if (!int.TryParse(input, out value)) Console.WriteLine("Malformed input, try again!");
            else
            {
                dto.value = value;
                break;
            }
        }

        HttpManager.WriteTagOutputValue(dto).Wait();
    }
        
    // TODO
    public void ShowCurrentTagValuesHandler() { }

    public void RegisterHandler()
    {
        RegisterDTO dto = new RegisterDTO();
        Console.Write("Enter your username: ");
        dto.username = Console.ReadLine().Trim();
            
        Console.Write("Enter your password: ");
        dto.password = Console.ReadLine().Trim();

        HttpManager.Register(dto).Wait();
    }

    public void LoginHandler()
    {
        LoginDTO dto = new LoginDTO();
            
        Console.Write("Enter your username: ");
        dto.username = Console.ReadLine().Trim();
            
        Console.Write("Enter your password: ");
        dto.password = Console.ReadLine().Trim();

        var task = HttpManager.Login(dto);
        task.Wait();
        this.token = task.Result;
        this.username = dto.username;
    }

    public void LogoutHandler()
    {
        LogoutDTO dto = new LogoutDTO();
        dto.token = token;
        dto.username = username;
        HttpManager.Logout(dto).Wait();
        this.token = "";
        this.username = "";
    }
}