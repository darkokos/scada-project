using System.Security.Cryptography;

namespace Common.RealTimeUnit;

public class AnalogValueDto(string tagName, decimal value, DateTime timestamp) {
    public string TagName { get; set; } = tagName;
    public decimal Value { get; set; } = value;
    public DateTime Timestamp { get; set; } = timestamp;
    public byte[]? HashValue { get; set; }
    public byte[]? Signature { get; set; }

    private byte[] SerializeMessage() {
        using var memoryStream = new MemoryStream();
        using (var binaryWriter = new BinaryWriter(memoryStream)) {
            binaryWriter.Write(TagName);
            binaryWriter.Write(Value);
            binaryWriter.Write(Timestamp.ToBinary());
        }
        return memoryStream.ToArray();
    }

    public void Sign(RSA key) {
        byte[] hashValue;
        using (var algorithm = SHA256.Create()) {
            hashValue = algorithm.ComputeHash(SerializeMessage());
        }
        HashValue = hashValue;
            
        var formatter = new RSAPKCS1SignatureFormatter(key);
        formatter.SetHashAlgorithm(nameof(SHA256));
        Signature = formatter.CreateSignature(HashValue);
    }

    public bool IsSignatureValid(RSA key) {
        if (HashValue == null || Signature == null)
            return false;
        
        var deformatter = new RSAPKCS1SignatureDeformatter(key);
        deformatter.SetHashAlgorithm(nameof(SHA256));
        return deformatter.VerifySignature(HashValue, Signature);
    }
}