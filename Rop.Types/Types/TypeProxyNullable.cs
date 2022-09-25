namespace Rop.Types.Types;

public class TypeProxyNullable : AbsTypeProxy
{
    public override Type Type { get; }
    public override ITypeProxy BaseType { get; }
    public override bool IsNullAllowed => true;
    public override bool IsBasicValueType => false;
    public override bool IsArray => false;
    public override bool IsNullable => true;
    public override bool IsList => false;
    public override bool IsEnumerable => false;
    public override bool IsEnum => false;
    public override bool IsString => false;
    public override bool HasEmptyConstructor => true;
    public override TypeCode TypeCode { get; }
    public override TypeType TypeType => TypeType.Nullable;
    public override object? GetDefaultValue() => null;
    public TypeProxyNullable(Type type, bool isnullallowed)
    {
        var subtype = Nullable.GetUnderlyingType(type);
        if (subtype is null) throw new ArgumentException($"Type {type} is not Nullable<>");
        Type = type;
        TypeCode = Type.GetTypeCode(type);
        BaseType = TypeProxy.Get(subtype);
    }
    public override string ToString() => $"{Type.Name}({BaseType.Name}){(IsNullAllowed ? "(?)" : "")}";
}