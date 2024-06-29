using AutoMapper;
using Common.RealTimeUnit;
using ScadaCore.Models;

namespace ScadaCore.Mappers;

public class AutoMapperConfiguration : Profile {
    public AutoMapperConfiguration() {
        CreateMap<AnalogInputTag, AnalogInputUnitDto>()
            .ForMember(
                destination => destination.TagName,
                act =>
                    act.MapFrom(source => source.Name)
            );
        CreateMap<AnalogOutputTag, AnalogOutputUnitDto>()
            .ForMember(
                destination => destination.CurrentValue,
                act =>
                    act.MapFrom(source => source.InitialValue)
            );
        CreateMap<DigitalInputTag, DigitalInputUnitDto>()
            .ForMember(
                destination => destination.TagName,
                act =>
                    act.MapFrom(source => source.Name)
            );
        CreateMap<DigitalOutputTag, DigitalOutputUnitDto>()
            .ForMember(
                destination => destination.CurrentValue,
                act =>
                    act.MapFrom(source => source.InitialValue)
            );
        CreateMap<AnalogValueDto, AnalogTagLog>()
            .ForMember(
                destination => destination.EmittedValue,
                act =>
                    act.MapFrom(source => source.Value)
            );
        CreateMap<DigitalValueDto, DigitalTagLog>()
            .ForMember(
                destination => destination.EmittedValue,
                act =>
                    act.MapFrom(source => source.Value)
            );
    }
}