namespace Common.DatabaseManagerCommon;

public class WriteTagValueDTO
{
    public string TagName { get; set; }
    public decimal? decimalValue { get; set; }
    public bool? boolValue { get; set; }
    public string token { get; set; }
    public string username { get; set; }
}