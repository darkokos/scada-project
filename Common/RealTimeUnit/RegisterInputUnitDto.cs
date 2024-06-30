using Lombok.NET;

namespace Common.RealTimeUnit;

[AllArgsConstructor(MemberType = MemberType.Property, AccessTypes = AccessTypes.Public)]
public partial class RegisterInputUnitDto {
    public string PublicKey { get; set; }
}