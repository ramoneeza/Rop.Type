namespace Rop.Types.Types;

public class TypeProxyEnum : ITypeProxy
{
    public Type Type { get; }
    public ITypeProxy BaseType { get; }
    public bool IsNullAllowed => false;
    public bool IsBasicValueType => false;
    public bool IsArray => false;
    public bool IsNullable => false;
    public bool IsList => false;
    public bool IsEnumerable => false;
    public bool IsEnum => true;
    public bool IsString => false;
    public bool HasEmptyConstructor => true;
    public TypeCode TypeCode { get; }
    public TypeType TypeType => TypeType.Enum;
    private readonly object _defaultvalue;
    public object? GetDefaultValue() => _defaultvalue;
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