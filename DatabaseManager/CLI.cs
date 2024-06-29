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

    public void AddTagHandler()
    {
        var dto = new AddTagDTO();
        dto.username = username;
        dto.token = token;
        int option;
        while (true)
        {
            Console.WriteLine("(0) Add Analog Input Tag");
            Console.WriteLine("(1) Add Analog Output Tag");
            Console.WriteLine("(2) Add Digital Input Tag");
            Console.WriteLine("(3) Add Digital Output Tag");
            Console.Write("> ");
            string input = Console.ReadLine().Trim();
            if (!int.TryParse(input, out option) || option < 0 || option > 3)
                Console.WriteLine("Malformed input, try again!");
            else break;
        }

        if (option == 0) dto.analogInput = readAnalogInput();
        else if (option == 1) dto.analogOutput = readAnalogOutput();
        else if (option == 2) dto.digitalInput = readDigitalInput();
        else dto.digitalOutput = readDigitalOutput();
        var task = HttpManager.AddTag(dto);
        task.Wait();
    }

    AddAnalogOutputTag readAnalogOutput()
    {
        var res = new AddAnalogOutputTag();
        Console.WriteLine("Enter tag name: ");
        res.Name = Console.ReadLine().Trim();
        Console.WriteLine("Enter tag description: ");
        res.Description = Console.ReadLine().Trim();
        int address;
        while (true) {
            Console.Write("Enter input output address: ");
            string input = Console.ReadLine().Trim();
            if (!int.TryParse(input, out address)) Console.WriteLine("Malformed input, try again!");
            else break;
        }
        res.InputOutputAddress = address;

        decimal invitialValue;
        while (true) {
            Console.Write("Enter initial value: ");
            string input = Console.ReadLine().Trim();
            if (!decimal.TryParse(input, out invitialValue)) Console.WriteLine("Malformed input, try again!");
            else break;
        }
        res.InitialValue = invitialValue;
        
        decimal lowLimit;
        while (true) {
            Console.Write("Enter low limit: ");
            string input = Console.ReadLine().Trim();
            if (!decimal.TryParse(input, out lowLimit)) Console.WriteLine("Malformed input, try again!");
            else break;
        }
        res.LowLimit = lowLimit;

        decimal highLimit;
        while (true) {
            Console.Write("Enter high limit: ");
            string input = Console.ReadLine().Trim();
            if (!decimal.TryParse(input, out highLimit)) Console.WriteLine("Malformed input, try again!");
            else break;
        }
        res.HighLimit = highLimit;
        
        Console.Write("Enter unit: ");
        res.Unit = Console.ReadLine().Trim();

        return res;
    }

    AddAnalogInputTag readAnalogInput()
    {
        var res = new AddAnalogInputTag();
        Console.WriteLine("Enter tag name: ");
        res.Name = Console.ReadLine().Trim();
        Console.WriteLine("Enter tag description: ");
        res.Description = Console.ReadLine().Trim();
        int address;
        while (true) {
            Console.Write("Enter input output address: ");
            string input = Console.ReadLine().Trim();
            if (!int.TryParse(input, out address)) Console.WriteLine("Malformed input, try again!");
            else break;
        }
        res.InputOutputAddress = address;

        bool isSimulated;
        while (true) {
            Console.Write("Is tag simulated: ");
            string input = Console.ReadLine().Trim();
            if (!bool.TryParse(input, out isSimulated)) Console.WriteLine("Malformed input, try again!");
            else break;
        }
        res.IsSimulated = isSimulated;
        
        bool isScanned;
        while (true) {
            Console.Write("Is tag scanned: ");
            string input = Console.ReadLine().Trim();
            if (!bool.TryParse(input, out isScanned)) Console.WriteLine("Malformed input, try again!");
            else break;
        }
        res.IsScanned = isScanned;

        int seconds;
        while (true) {
            Console.Write("Enter scant time in seconds: ");
            string input = Console.ReadLine().Trim();
            if (!int.TryParse(input, out seconds)) Console.WriteLine("Malformed input, try again!");
            else break;
        }
        res.ScanTime = new TimeSpan(0, 0, seconds);
        res.AlarmIds = new HashSet<int>();
        
        decimal lowLimit;
        while (true) {
            Console.Write("Enter low limit: ");
            string input = Console.ReadLine().Trim();
            if (!decimal.TryParse(input, out lowLimit)) Console.WriteLine("Malformed input, try again!");
            else break;
        }
        res.LowLimit = lowLimit;

        decimal highLimit;
        while (true) {
            Console.Write("Enter high limit: ");
            string input = Console.ReadLine().Trim();
            if (!decimal.TryParse(input, out highLimit)) Console.WriteLine("Malformed input, try again!");
            else break;
        }
        res.HighLimit = highLimit;
        
        Console.Write("Enter unit: ");
        res.Unit = Console.ReadLine().Trim();
        
        return res;
    }

    AddDigitalInputTag readDigitalInput()
    {
        var res = new AddDigitalInputTag();
        Console.WriteLine("Enter tag name: ");
        res.Name = Console.ReadLine().Trim();
        Console.WriteLine("Enter tag description: ");
        res.Description = Console.ReadLine().Trim();
        int address;
        while (true) {
            Console.Write("Enter input output address: ");
            string input = Console.ReadLine().Trim();
            if (!int.TryParse(input, out address)) Console.WriteLine("Malformed input, try again!");
            else break;
        }
        res.InputOutputAddress = address;

        bool isSimulated;
        while (true) {
            Console.Write("Is tag simulated: ");
            string input = Console.ReadLine().Trim();
            if (!bool.TryParse(input, out isSimulated)) Console.WriteLine("Malformed input, try again!");
            else break;
        }
        res.IsSimulated = isSimulated;
        
        bool isScanned;
        while (true) {
            Console.Write("Is tag scanned: ");
            string input = Console.ReadLine().Trim();
            if (!bool.TryParse(input, out isScanned)) Console.WriteLine("Malformed input, try again!");
            else break;
        }
        res.IsScanned = isScanned;

        int seconds;
        while (true) {
            Console.Write("Enter scant time in seconds: ");
            string input = Console.ReadLine().Trim();
            if (!int.TryParse(input, out seconds)) Console.WriteLine("Malformed input, try again!");
            else break;
        }
        res.ScanTime = new TimeSpan(0, 0, seconds);
        return res;
    }

    AddDigitalOutputTag readDigitalOutput()
    {
        var res = new AddDigitalOutputTag();
        Console.WriteLine("Enter tag name: ");
        res.Name = Console.ReadLine().Trim();
        Console.WriteLine("Enter tag description: ");
        res.Description = Console.ReadLine().Trim();
        int address;
        while (true) {
            Console.Write("Enter input output address: ");
            string input = Console.ReadLine().Trim();
            if (!int.TryParse(input, out address)) Console.WriteLine("Malformed input, try again!");
            else break;
        }
        res.InputOutputAddress = address;

        bool intialValue;
        while (true) {
            Console.Write("Enter tag initial value: ");
            string input = Console.ReadLine().Trim();
            if (!bool.TryParse(input, out intialValue)) Console.WriteLine("Malformed input, try again!");
            else break;
        }
        res.InitialValue = intialValue;
        
        return res;
    }

    public void RemoveTagHandler()
    {
        DeleteTagDTO dto = new DeleteTagDTO();
        dto.token = token;
        dto.username = username;
        Console.Write("Enter name of the tag to be deleted: ");
        dto.TagName = Console.ReadLine().Trim();
        HttpManager.DeleteTag(dto).Wait();
    }

    public void ChangeTagScanningHandler()
    {
        ChangeScanTagDTO dto = new ChangeScanTagDTO();
        dto.token = token;
        dto.username = username;
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
        Console.Write("Enter name of the tag: ");
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
    public void ShowCurrentTagValuesHandler()
    {
        var dto = new ShowCurrentTagValuesDTO();
        dto.token = token;
        dto.username = username;
        var task = HttpManager.GetCurrentTagValues(dto);
        task.Wait();
        Console.WriteLine(task.Result);
    }

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