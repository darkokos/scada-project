using Lombok.NET;

namespace Common.RealTimeUnit;

[AllArgsConstructor(MemberType = MemberType.Property, AccessTypes = AccessTypes.Public)]
public partial class AnalogValueDto {
    public string TagName { get; set; }
    public decimal Value { get; set; }
}