using Common.RealTimeUnit;
using ScadaCore.Utils;

namespace ScadaCore.Drivers;

public interface IAnalogRealTimeDriver : IRealTimeDriver {
    AnalogValueDto? Read(int inputAddress);
    
    ServiceResponse<AnalogValueDto> Write(int outputAddress, AnalogValueDto value);

    void ClearValues();
}