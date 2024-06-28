using Lombok.NET;

namespace Common.RealTimeUnit;

[AllArgsConstructor(MemberType = MemberType.Property, AccessTypes = AccessTypes.Public)]
public partial class DigitalValueDto {
    public string TagName { get; set; }
    public bool Value { get; set; }
}