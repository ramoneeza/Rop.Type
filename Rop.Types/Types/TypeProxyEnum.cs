namespace Rop.Types.Types;

public class TypeProxyEnum : AbsTypeProxy
{
    public override Type Type { get; }
    public override ITypeProxy BaseType { get; }
    public override bool IsNullAllowed => false;
    public override bool IsBasicValueType => false;
    public override bool IsArray => false;
    public override bool IsNullable => false;
    public override bool IsList => false;
    public override bool IsEnumerable => false;
    public override bool IsEnum => true;
    public override bool IsString => false;
    public override bool HasEmptyConstructor => true;
    public override TypeCode TypeCode { get; }
    public override TypeType TypeType => TypeType.Enum;
    private readonly object _defaultvalue;
    public override object? GetDefaultValue() => _defaultvalue;
    public TypeProxyEnum(Type type, bool isnullallowed)
    {
        var subtype = type.GetEnumUnderlyingType();
        if (subtype is null) throw new ArgumentException($"Type {type} is not Enum");
        Type = type;
        TypeCode = Type.GetTypeCode(type);
        BaseType = TypeProxy.Get(subtype);
        _defaultvalue = Activator.CreateInstance(type)!;
    }
    public override string ToString() => $"{Type.Name}({BaseType.Name}){(IsNullAllowed ? "(?)" : "")}";
}