using System.Data;

namespace Rop.Types.Types;

public class TypeProxyString : AbsTypeProxy
{
    public override Type Type { get; }
    public override ITypeProxy? BaseType => null;
    public override bool IsNullAllowed { get; }
    public override bool IsBasicValueType => false;
    public override bool IsArray => false;
    public override bool IsNullable => false;
    public override bool IsList => false;
    public override bool IsEnumerable => false;
    public override bool IsEnum => false;
    public override bool IsString => true;
    public override bool HasEmptyConstructor => true;
    public override TypeCode TypeCode { get; }
    public override TypeType TypeType => TypeType.String;
    private readonly object? _defaultvalue;
    public override object? GetDefaultValue() => _defaultvalue;
    public TypeProxyString(Type type, bool isnullallowed)
    {
        Type = type;
        var typeCode = Type.GetTypeCode(type);
        if (typeCode != TypeCode.String) throw new ArgumentException($"Type {type} is not String");
        TypeCode = typeCode;
        _defaultvalue = isnullallowed ? null : "";
        IsNullAllowed = isnullallowed;
    }
    public override string ToString() => (BaseType == null) ? $"{Type.Name}{(IsNullAllowed ? "(?)" : "")}" : $"{Type.Name}({BaseType.Name}){(IsNullAllowed ? "(?)" : "")}";
}