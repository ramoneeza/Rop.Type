namespace Rop.Types.Types;

public class TypeProxyBasicValue : ITypeProxy
{
    public Type Type { get; }
    public ITypeProxy? BaseType => null;
    public bool IsNullAllowed => false;
    public bool IsBasicValueType => true;
    public bool IsArray => false;
    public bool IsNullable => false;
    public bool IsList => false;
    public bool IsEnumerable => false;
    public bool IsEnum => false;
    public bool IsString => false;
    public bool HasEmptyConstructor => true;
    public TypeCode TypeCode { get; }
    public TypeType TypeType => TypeType.BasicValue;
    private readonly object _defaultvalue;
    public object? GetDefaultValue() => _defaultvalue;
    public TypeProxyBasicValue(Type type, bool isnullallowed)
    {
        Type = type;
        TypeCode = Type.GetTypeCode(type);
        _defaultvalue = Activator.CreateInstance(type)!;
    }

    public override string ToString() => $"{Type.Name}";
}