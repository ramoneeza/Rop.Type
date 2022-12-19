namespace Rop.Types.Types;

public class TypeProxyBasicValue : AbsTypeProxy
{
    public override string FriendlyName { get; }
    public override Type Type { get; }
    public override ITypeProxy? BaseType => null;
    public override bool IsNullAllowed => false;
    public override bool IsBasicValueType => true;
    public override bool IsArray => false;
    public override bool IsNullable => false;
    public override bool IsList => false;
    public override bool IsEnumerable => false;
    public override bool IsEnum => false;
    public override bool IsString => false;
    public override bool HasEmptyConstructor => true;
    public override TypeCode TypeCode { get; }
    public override TypeType TypeType => TypeType.BasicValue;
    private readonly object _defaultvalue;
    public override object? GetDefaultValue() => _defaultvalue;
    public TypeProxyBasicValue(Type type, bool isnullallowed)
    {
        Type = type;
        TypeCode = Type.GetTypeCode(type);
        _defaultvalue = Activator.CreateInstance(type)!;
        FriendlyName = type.Name;
    }

    public override string ToString() => $"{Type.Name}";
}