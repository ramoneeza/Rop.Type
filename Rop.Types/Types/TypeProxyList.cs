namespace Rop.Types.Types;

public class TypeProxyList : ITypeProxy
{
    public Type Type { get; }
    public ITypeProxy BaseType { get; }
    public bool IsNullAllowed { get; }
    public bool IsBasicValueType => false;
    public bool IsArray => false;
    public bool IsNullable => false;
    public bool IsList => true;
    public bool IsEnumerable => true;
    public bool IsEnum => false;
    public bool IsString => false;
    public bool HasEmptyConstructor { get; }
    public bool IsReadOnly { get; }
    public TypeCode TypeCode { get; }
    public TypeType TypeType => TypeType.List;
    private readonly Func<object?> _defaultvalue;
    private Type _primitivetype;
    public object? GetDefaultValue() => _defaultvalue();
    public TypeProxyList(Type type, bool isnullallowed)
    {
        var lst = type.HasGenericInterface(typeof(IList<>));
        var ro = type.HasGenericInterface(typeof(IReadOnlyList<>));
        var baseType = (lst ?? ro) ?? throw new ArgumentException($"Type {type} is not a List");
        Type = type;
        TypeCode = Type.GetTypeCode(type);
        BaseType = TypeProxy.Get(baseType);
        IsReadOnly = lst == null;
        HasEmptyConstructor = type.HasDefaultConstructor();
        if (isnullallowed)
            _defaultvalue = () => null;
        else
        {
            if (HasEmptyConstructor)
                _defaultvalue = () => null;
            else
            {
                _primitivetype = typeof(List<>).MakeGenericType(baseType);
                _defaultvalue = () => Activator.CreateInstance(_primitivetype);
            }
        }
    }
    public override string ToString() => $"{Type.Name}({BaseType.Name}){(IsNullAllowed ? "(?)" : "")}";
}