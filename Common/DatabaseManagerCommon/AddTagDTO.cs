namespace Common.DatabaseManagerCommon;

public class AddTagDTO
{
    public AddAnalogInputTag? analogInput { get; set;  }
    public AddAnalogOutputTag? analogOutput { get; set; }
    public AddDigitalInputTag? digitalInput { get; set; }
    public AddDigitalOutputTag? digitalOutput { get; set; }
    public string token { get; set; }
    public string username { get; set; }
}