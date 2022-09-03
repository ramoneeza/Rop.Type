namespace Rop.Types.Types;

public class TypeProxyNullable : ITypeProxy
{
    public Type Type { get; }
    public ITypeProxy BaseType { get; }
    public bool IsNullAllowed => true;
    public bool IsBasicValueType => false;
    public bool IsArray => false;
    public bool IsNullable => true;
    public bool IsList => false;
    public bool IsEnumerable => false;
    public bool IsEnum => false;
    public bool IsString => false;
    public bool HasEmptyConstructor => true;
    public TypeCode TypeCode { get; }
    public TypeType TypeType => TypeType.Nullable;
    public object? GetDefaultValue() => null;
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