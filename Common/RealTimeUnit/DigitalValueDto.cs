using System.Security.Cryptography;
using System.Text;

namespace Common.RealTimeUnit;

public class DigitalValueDto(string tagName, bool value, DateTime timestamp) {
    public string TagName { get; set; } = tagName;
    public bool Value { get; set; } = value;
    public DateTime Timestamp { get; set; } = timestamp;
    public string? Signature { get; set; }
    
    private byte[] SerializeMessage() {
        using var memoryStream = new MemoryStream();
        using (var binaryWriter = new BinaryWriter(memoryStream)) {
            binaryWriter.Write(TagName);
            binaryWriter.Write(Value);
            binaryWriter.Write(Timestamp.ToBinary());
        }
        return memoryStream.ToArray();
    }

    public void Sign(AsymmetricAlgorithm key) {
        var hashValue = SHA256.HashData(SerializeMessage());

        RSAPKCS1SignatureFormatter? formatter;
        using (key) {
            formatter = new RSAPKCS1SignatureFormatter(key);
        }
        formatter.SetHashAlgorithm("SHA256");
        Signature = Encoding.UTF8.GetString(formatter.CreateSignature(hashValue));
    }
}