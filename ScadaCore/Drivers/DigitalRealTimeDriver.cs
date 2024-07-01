using System.Collections.Concurrent;
using System.Net;
using System.Security.Cryptography;
using Common.RealTimeUnit;
using ScadaCore.Utils;

namespace ScadaCore.Drivers;

public class DigitalRealTimeDriver : IDigitalRealTimeDriver {
    private static readonly ConcurrentDictionary<int, string> WriterKeys = new();
    private static readonly ConcurrentDictionary<int, DigitalValueDto> Values = new();

    public void RegisterWriter(int inputAddress, string publicKey) {
        WriterKeys[inputAddress] = publicKey;
    }
    
    public DigitalValueDto? Read(int inputAddress) {
        return Values.GetValueOrDefault(inputAddress);
    }
    
    private static bool IsSignatureValid(string? publicKey, DigitalValueDto value) {
        if (publicKey == null)
            return false;
        
        var key = new RSACryptoServiceProvider(new CspParameters());
        key.FromXmlString(publicKey);
        return value.IsSignatureValid(key);
    }

    public ServiceResponse<DigitalValueDto> Write(int outputAddress, DigitalValueDto value) {
        if (!IsSignatureValid(WriterKeys.GetValueOrDefault(outputAddress), value))
            return new ServiceResponse<DigitalValueDto>(
                HttpStatusCode.BadRequest,
                "The message integrity was compromised."
            );
        
        Values[outputAddress] = value;
        return new ServiceResponse<DigitalValueDto>(value, HttpStatusCode.OK);
    }
}