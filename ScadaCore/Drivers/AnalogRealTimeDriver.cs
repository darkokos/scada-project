using System.Collections.Concurrent;
using System.Net;
using System.Security.Cryptography;
using Common.RealTimeUnit;
using ScadaCore.Utils;

namespace ScadaCore.Drivers;

public class AnalogRealTimeDriver : IAnalogRealTimeDriver {
    private static readonly ConcurrentDictionary<int, string> WriterKeys = new();
    private static readonly ConcurrentDictionary<int, AnalogValueDto> Values = new();

    public void RegisterWriter(int inputAddress, string publicKey) {
        WriterKeys[inputAddress] = publicKey;
    }

    public AnalogValueDto? Read(int inputAddress) {
        return Values.GetValueOrDefault(inputAddress);
    }

    private static bool IsSignatureValid(string? publicKey, AnalogValueDto value) {
        if (publicKey == null)
            return false;
        
        var key = new RSACryptoServiceProvider(new CspParameters());
        key.FromXmlString(publicKey);
        return value.IsSignatureValid(key);
    }

    public ServiceResponse<AnalogValueDto> Write(int outputAddress, AnalogValueDto value) {
        if (!IsSignatureValid(WriterKeys.GetValueOrDefault(outputAddress), value))
            return new ServiceResponse<AnalogValueDto>(
                HttpStatusCode.BadRequest,
                "The message integrity was compromised."
            );
        
        Values[outputAddress] = value;
        return new ServiceResponse<AnalogValueDto>(value, HttpStatusCode.OK);
    }

    public void ClearValues() {
        Values.Clear();
    }
}