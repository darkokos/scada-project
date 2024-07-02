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

    public async Task<ICollection<Tag>> GetAllInputTags() {
        return await tagRepository.GetAllInputTags();
    }
    
    public async Task<ServiceResponse<RtuInformationDto>> GetTagForRtuAsync(string name) {
        var tag = await tagRepository.GetTagAsync(name);
        if (tag == null)
            return new ServiceResponse<RtuInformationDto>(HttpStatusCode.NotFound, "Tag not found.");
        
        return tag switch {
            AnalogInputTag analogInputTag =>
                analogInputTag.IsSimulated ?
                    new ServiceResponse<RtuInformationDto>(
                        HttpStatusCode.BadRequest,
                        "Found tag does not correspond to a real-time unit."
                    )
                    : new ServiceResponse<RtuInformationDto>(
                        new RtuInformationDto(analogInputTag.Name, true, true),
                        HttpStatusCode.OK
                    ),
            AnalogOutputTag analogOutputTag =>
                new ServiceResponse<RtuInformationDto>(
                    new RtuInformationDto(analogOutputTag.Name, true, false),
                    HttpStatusCode.OK
                ),
            DigitalInputTag digitalInputTag =>
                digitalInputTag.IsSimulated ?
                    new ServiceResponse<RtuInformationDto>(
                        HttpStatusCode.BadRequest, 
                        "Found tag does not correspond to a real-time unit."
                    )
                    : new ServiceResponse<RtuInformationDto>(
                        new RtuInformationDto(digitalInputTag.Name, false, true),
                        HttpStatusCode.OK
                    ),
            DigitalOutputTag digitalOutputTag =>
                new ServiceResponse<RtuInformationDto>(
                    new RtuInformationDto(digitalOutputTag.Name, false, false),
                    HttpStatusCode.OK
                ),
            _ => new ServiceResponse<RtuInformationDto>(HttpStatusCode.NotFound, "Tag not found.")
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

    public async Task<ServiceResponse<Tag>> CreateTagAsync(Tag tag) {
        if (await tagRepository.GetTagAsync(tag.Name) != null)
            return new ServiceResponse<Tag>(HttpStatusCode.BadRequest, "Tag with such name already exists.");
        
        if (await tagRepository.GetTagByInputOutputAddressAsync(tag.InputOutputAddress) != null)
            return new ServiceResponse<Tag>(
                HttpStatusCode.BadRequest,
                "Another tag receives/sends values from/to such an address."
            );
            
        var createdTag = await tagRepository.CreateTagAsync(tag);
        if (createdTag == null)
            return new ServiceResponse<Tag>(
                HttpStatusCode.InternalServerError,
                "Something went wrong while creating the tag."
            );
        return new ServiceResponse<Tag>(createdTag, HttpStatusCode.OK);
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