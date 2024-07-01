using System.Net;
using AutoMapper;
using Common.RealTimeUnit;
using ScadaCore.Models;
using ScadaCore.Repositories;
using ScadaCore.Utils;
using System.Collections.ObjectModel;
using ScadaCore.Drivers;

namespace ScadaCore.Services;

public class TagService(
    ITagRepository tagRepository,
    IMapper mapper,
    IAnalogRealTimeDriver analogRealTimeDriver,
    IDigitalRealTimeDriver digitalRealTimeDriver
) : ITagService {
    public async Task<Tag?> GetTagAsync(string name)
    {
        var task = tagRepository.GetTagAsync(name);
        task.Wait();
        return task.Result;
    }
    
    public async Task<RtuInformationDto?> GetTagForRtuAsync(string name) {
        var tag = await tagRepository.GetTagAsync(name);
        return tag == null ? null : tag switch {
            AnalogInputTag analogInputTag => new RtuInformationDto(analogInputTag.Name, true, true),
            AnalogOutputTag analogOutputTag => new RtuInformationDto(analogOutputTag.Name, true, false),
            DigitalInputTag digitalInputTag => new RtuInformationDto(digitalInputTag.Name, false, true),
            DigitalOutputTag digitalOutputTag =>
                new RtuInformationDto(digitalOutputTag.Name, false, false),
            _ => null
        };
    }

    public async Task<ServiceResponse<AnalogInputUnitDto>> GetAnalogInputTagAsync(
        string name,
        RegisterInputUnitDto dto
    ) {
        var tag = await tagRepository.GetTagAsync(name);
        if (tag == null)
            return new ServiceResponse<AnalogInputUnitDto>(HttpStatusCode.NotFound, "Tag not found.");
        
        if (tag.GetType() != typeof(AnalogInputTag))
            return new ServiceResponse<AnalogInputUnitDto>(
                HttpStatusCode.BadRequest,
                "Wrong tag type."
            );
        
        analogRealTimeDriver.RegisterWriter(tag.InputOutputAddress, dto.PublicKey);
        return new ServiceResponse<AnalogInputUnitDto>(
            mapper.Map<AnalogInputUnitDto>((AnalogInputTag) tag),
            HttpStatusCode.OK
        );
    }

    public async Task<ServiceResponse<AnalogOutputUnitDto>> GetAnalogOutputTagAsync(string name) {
        var tag = await tagRepository.GetTagAsync(name);
        if (tag == null)
            return new ServiceResponse<AnalogOutputUnitDto>(HttpStatusCode.NotFound, "Tag not found.");
        
        if (tag.GetType() != typeof(AnalogOutputTag))
            return new ServiceResponse<AnalogOutputUnitDto>(
                HttpStatusCode.BadRequest,
                "Wrong tag type."
            );
        
        return new ServiceResponse<AnalogOutputUnitDto>(
            mapper.Map<AnalogOutputUnitDto>((AnalogOutputTag) tag),
            HttpStatusCode.OK
        );
    }
    
    public async Task<ServiceResponse<DigitalInputUnitDto>> GetDigitalInputTagAsync(
        string name,
        RegisterInputUnitDto dto
    ) {
        var tag = await tagRepository.GetTagAsync(name);
        if (tag == null)
            return new ServiceResponse<DigitalInputUnitDto>(HttpStatusCode.NotFound, "Tag not found.");
        
        if (tag.GetType() != typeof(DigitalInputTag))
            return new ServiceResponse<DigitalInputUnitDto>(
                HttpStatusCode.BadRequest,
                "Wrong tag type."
            );
        
        digitalRealTimeDriver.RegisterWriter(tag.InputOutputAddress, dto.PublicKey);
        return new ServiceResponse<DigitalInputUnitDto>(
            mapper.Map<DigitalInputUnitDto>((DigitalInputTag) tag),
            HttpStatusCode.OK
        );
    }
    
    public async Task<ServiceResponse<DigitalOutputUnitDto>> GetDigitalOutputTagAsync(string name) {
        var tag = await tagRepository.GetTagAsync(name);
        if (tag == null)
            return new ServiceResponse<DigitalOutputUnitDto>(HttpStatusCode.NotFound, "Tag not found.");
        
        if (tag.GetType() != typeof(DigitalOutputTag))
            return new ServiceResponse<DigitalOutputUnitDto>(
                HttpStatusCode.BadRequest,
                "Wrong tag type."
            );
        
        return new ServiceResponse<DigitalOutputUnitDto>(
            mapper.Map<DigitalOutputUnitDto>((DigitalOutputTag) tag),
            HttpStatusCode.OK
        );
    }

    public async Task<Tag?> CreateTagAsync(Tag tag)
    {
        var task = tagRepository.CreateTagAsync(tag);
        task.Wait();
        return task.Result;
    }

    public async Task<bool> DeleteTagAsync(Tag tag)
    {
        var task = tagRepository.DeleteTagAsync(tag);
        task.Wait();
        return task.Result;
    }

    public async Task<Collection<Tag>> GetAllOutputTags()
    {
        var task = tagRepository.GetAllOutputTags();
        task.Wait();
        return task.Result;
    }
}