namespace Rop.Types.Types;

public class TypeProxyArray : ITypeProxy
{
    public Type Type { get; }
    public ITypeProxy BaseType { get; }
    public bool IsNullAllowed { get; }
    public bool IsBasicValueType => false;
    public bool IsArray => true;
    public bool IsNullable => false;
    public bool IsList => false;
    public bool IsEnumerable => true;
    public bool IsEnum => false;
    public bool IsString => false;
    public bool HasEmptyConstructor => false;
    public TypeCode TypeCode { get; }
    public TypeType TypeType => TypeType.Array;
    private readonly object? _defaultvalue;
    public object? GetDefaultValue() => IsNullAllowed ? null : Array.CreateInstance(BaseType.Type, 0);
    public TypeProxyArray(Type type, bool isnullallowed)
    {
        if (!type.IsArray) throw new ArgumentException($"Type {type} is not Array");
        Type = type;
        TypeCode = Type.GetTypeCode(type);
        var baseType = type.GetElementType()!;
        BaseType = TypeProxy.Get(baseType);
        IsNullAllowed = isnullallowed;
        _defaultvalue = isnullallowed ? null : Array.CreateInstance(baseType, 0);

    }
    public override string ToString() => $"{Type.Name}({BaseType.Name}){(IsNullAllowed ? "(?)" : "")}";
}