using System.Net;
using AutoMapper;
using Common.RealTimeUnit;
using ScadaCore.Models;
using ScadaCore.Repositories;
using ScadaCore.Utils;

namespace ScadaCore.Services;

public class TagService(ITagRepository tagRepository, IMapper mapper) : ITagService {
    public async Task<Tag?> GetTagAsync(string name) {
        return null;
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

    private async Task<ServiceResponse<TDestination>> GetTagAsync<TSource, TDestination>(string name)
        where TSource : Tag
        where TDestination : class {
        var tag = await tagRepository.GetTagAsync(name);
        if (tag == null)
            return new ServiceResponse<TDestination>(HttpStatusCode.NotFound, "Tag not found.");

        if (tag.GetType() != typeof(TSource))
            return new ServiceResponse<TDestination>(
                HttpStatusCode.BadRequest,
                "Wrong tag type."
            );

        return new ServiceResponse<TDestination>(mapper.Map<TDestination>((TSource) tag), HttpStatusCode.OK);
    }
    
    public async Task<ServiceResponse<AnalogInputUnitDto>> GetAnalogInputTagAsync(string name) {
        return await GetTagAsync<AnalogInputTag, AnalogInputUnitDto>(name);
    }

    public async Task<ServiceResponse<AnalogOutputUnitDto>> GetAnalogOutputTagAsync(string name) {
        return await GetTagAsync<AnalogOutputTag, AnalogOutputUnitDto>(name);
    }
    
    public async Task<ServiceResponse<DigitalInputUnitDto>> GetDigitalInputTagAsync(string name) {
        return await GetTagAsync<DigitalInputTag, DigitalInputUnitDto>(name);
    }
    
    public async Task<ServiceResponse<DigitalOutputUnitDto>> GetDigitalOutputTagAsync(string name) {
        return await GetTagAsync<DigitalOutputTag, DigitalOutputUnitDto>(name);
    }

    public async Task<Tag?> CreateTagAsync(Tag tag) {
        return null;
    }

    public async Task<bool> DeleteTagAsync(Tag tag) {
        return default;
    }
}