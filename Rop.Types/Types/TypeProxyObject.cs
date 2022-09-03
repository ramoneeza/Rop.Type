namespace Rop.Types.Types;

public class TypeProxyObject : ITypeProxy
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
    public bool IsString => false;
    public bool HasEmptyConstructor { get; }
    public TypeCode TypeCode { get; }
    public TypeType TypeType => TypeType.Object;
    private readonly Func<object?> _defaultvalue;
    public object? GetDefaultValue() => _defaultvalue();
    public TypeProxyObject(Type type, bool isnullallowed)
    {
        Type = type;
        TypeCode = Type.GetTypeCode(type);
        HasEmptyConstructor = type.HasDefaultConstructor();
        IsNullAllowed = isnullallowed;
        if (isnullallowed)
            _defaultvalue = () => null;
        else
        {
            if (HasEmptyConstructor)
                _defaultvalue = () => Activator.CreateInstance(Type);
            else
            {
                _defaultvalue = () => null; // Can't do nothing more
            }
        }
    }
    public override string ToString() => (BaseType == null) ? $"{Type.Name}{(IsNullAllowed ? "(?)" : "")}" : $"{Type.Name}({BaseType.Name}){(IsNullAllowed ? "(?)" : "")}";
}