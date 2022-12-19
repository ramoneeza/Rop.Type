namespace Rop.Types.Types;

public class TypeProxyEnumerable : AbsTypeProxy
{
    public override Type Type { get; }
    public override string FriendlyName { get; }
    public override ITypeProxy BaseType { get; }
    public override bool IsNullAllowed { get; }
    public override bool IsBasicValueType => false;
    public override bool IsArray => false;
    public override bool IsNullable => false;
    public override bool IsList => false;
    public override bool IsEnumerable => true;
    public override bool IsEnum => false;
    public override bool IsString => false;
    public override bool HasEmptyConstructor { get; }
    public override TypeCode TypeCode { get; }
    public override TypeType TypeType => TypeType.Enumerable;
    private readonly Type? _primitivetype=null;
    private readonly Func<object?> _defaultvalue=()=>null;
    public override object? GetDefaultValue() => _defaultvalue();

    public TypeProxyEnumerable(Type type, bool isnullallowed)
    {
        var lst = type.HasGenericInterface(typeof(IEnumerable<>));
        var baseType = lst ?? throw new ArgumentException($"Type {type} is not a IEnumerable<>");
        Type = type;
        FriendlyName = type.Name;
        TypeCode = Type.GetTypeCode(type);
        BaseType = TypeProxy.Get(baseType);
        IsNullAllowed = isnullallowed;
        var hasemptyconstructor= type.HasDefaultConstructor();
        HasEmptyConstructor = hasemptyconstructor;

        if (!isnullallowed)
        {
            if (hasemptyconstructor)
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