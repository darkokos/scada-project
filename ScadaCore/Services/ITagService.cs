using Common.RealTimeUnit;
using ScadaCore.Models;
using ScadaCore.Utils;

namespace ScadaCore.Services;

public interface ITagService {
    Task<Tag?> GetTagAsync(string name);
    
    Task<RtuInformationDto?> GetTagForRtuAsync(string name);
    
    Task<ServiceResponse<AnalogInputUnitDto>> GetAnalogInputTagAsync(string name);
    
    Task<ServiceResponse<AnalogOutputUnitDto>> GetAnalogOutputTagAsync(string name);
    
    Task<ServiceResponse<DigitalInputUnitDto>> GetDigitalInputTagAsync(string name);
    
    Task<ServiceResponse<DigitalOutputUnitDto>> GetDigitalOutputTagAsync(string name);
    
    Task<Tag?> CreateTagAsync(Tag tag);

    Task<bool> DeleteTagAsync(Tag tag);
}