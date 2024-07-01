using Common.RealTimeUnit;
using ScadaCore.Utils;

namespace ScadaCore.Drivers;

public interface IDigitalRealTimeDriver : IRealTimeDriver {
    DigitalValueDto? Read(int inputOutputAddress);
    
    ServiceResponse<DigitalValueDto> Write(int outputAddress, DigitalValueDto value);
}