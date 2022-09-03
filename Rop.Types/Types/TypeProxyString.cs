using System.Data;

namespace Rop.Types.Types;

public class TypeProxyString : ITypeProxy
{
    public Type Type { get; }
    public ITypeProxy? BaseType => null;
    public bool IsNullAllowed { get; }
    public bool IsBasicValueType => false;
    public bool IsArray => false;
    public bool IsNullable => false;
    public bool IsList => false;
    public bool IsEnumerable => false;
    public bool IsEnum => false;
    public bool IsString => true;
    public bool HasEmptyConstructor => true;
    public TypeCode TypeCode { get; }
    public TypeType TypeType => TypeType.String;
    private readonly object? _defaultvalue;
    public object? GetDefaultValue() => _defaultvalue;
    public TypeProxyString(Type type, bool isnullallowed)
    {
        Type = type;
        TypeCode = Type.GetTypeCode(type);
        if (TypeCode != TypeCode.String) throw new ArgumentException($"Type {type} is not Enum");
        _defaultvalue = isnullallowed ? null : "";
    }
    public override string ToString() => (BaseType == null) ? $"{Type.Name}{(IsNullAllowed ? "(?)" : "")}" : $"{Type.Name}({BaseType.Name}){(IsNullAllowed ? "(?)" : "")}";
}