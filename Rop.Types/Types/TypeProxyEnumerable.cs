namespace Rop.Types.Types;

public class TypeProxyEnumerable : ITypeProxy
{
    public Type Type { get; }
    public ITypeProxy BaseType { get; }
    public bool IsNullAllowed { get; }
    public bool IsBasicValueType => false;
    public bool IsArray => false;
    public bool IsNullable => false;
    public bool IsList => false;
    public bool IsEnumerable => true;
    public bool IsEnum => false;
    public bool IsString => false;
    public bool HasEmptyConstructor { get; }
    public TypeCode TypeCode { get; }
    public TypeType TypeType => TypeType.Enumerable;
    private readonly Func<object?> _defaultvalue;
    private Type _primitivetype;
    public object? GetDefaultValue() => _defaultvalue();
    public TypeProxyEnumerable(Type type, bool isnullallowed)
    {
        var lst = type.HasGenericInterface(typeof(IEnumerable<>));
        var baseType = lst ?? throw new ArgumentException($"Type {type} is not a IEnumerable<>");
        Type = type;
        TypeCode = Type.GetTypeCode(type);
        BaseType = TypeProxy.Get(baseType);
        IsNullAllowed = isnullallowed;
        HasEmptyConstructor = type.HasDefaultConstructor();
        if (isnullallowed)
            _defaultvalue = () => null;
        else
        {
            if (HasEmptyConstructor)
                _defaultvalue = () => Activator.CreateInstance(Type);
            else
            {
                _primitivetype = typeof(List<>).MakeGenericType(baseType);
                _defaultvalue = () => Activator.CreateInstance(_primitivetype);
            }
        }
    }
    public override string ToString() => $"{Type.Name}({BaseType.Name}){(IsNullAllowed ? "(?)" : "")}";
}