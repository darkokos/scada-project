using Lombok.NET;

namespace Common.RealTimeUnit;

[AllArgsConstructor(MemberType = MemberType.Property, AccessTypes = AccessTypes.Public)]
public partial class RtuInformationDto {
    public string TagName { get; set; }
    public bool IsAnalog { get; set; }
    public bool IsInput { get; set; }
}