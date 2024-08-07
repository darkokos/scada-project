﻿using Common.RealTimeUnit;
using ScadaCore.Models;
using ScadaCore.Utils;
using System.Collections.ObjectModel;

namespace ScadaCore.Services;

public interface ITagService {
    Task<Tag?> GetTagAsync(string name);

    Task<ICollection<Tag>> GetAllInputTags();
    
    Task<ServiceResponse<RtuInformationDto>> GetTagForRtuAsync(string name);
    
    Task<ServiceResponse<AnalogInputUnitDto>> GetAnalogInputTagAsync(string name, RegisterInputUnitDto dto);
    
    Task<ServiceResponse<AnalogOutputUnitDto>> GetAnalogOutputTagAsync(string name);
    
    Task<ServiceResponse<DigitalInputUnitDto>> GetDigitalInputTagAsync(string name, RegisterInputUnitDto dto);
    
    Task<ServiceResponse<DigitalOutputUnitDto>> GetDigitalOutputTagAsync(string name);
    
    Task<ServiceResponse<Tag>> CreateTagAsync(Tag tag);

    Task<bool> DeleteTagAsync(Tag tag);

    Task<Collection<Tag>> GetAllOutputTags();
}