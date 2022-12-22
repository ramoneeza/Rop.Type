namespace Rop.Types.Types;

public class TypeProxyList : AbsTypeProxy
{
    public sealed override ITypeProxy BaseType { get; }
    public override bool IsBasicValueType => false;
    public override bool IsArray => false;
    public override bool IsNullable => false;
    public override bool IsList => true;
    public override bool IsEnumerable => true;
    public override bool IsEnum => false;
    public override bool IsString => false;
    public override bool HasEmptyConstructor { get; }
    public bool IsReadOnly { get; }
    public override TypeCode TypeCode { get; }
    public override TypeType TypeType => TypeType.List;
    private readonly Func<object?> _defaultvalue;
    public override object? GetDefaultValue() => _defaultvalue();
    public TypeProxyList(Type type, bool isnullallowed):base(type,isnullallowed) 
    {
        var lst = type.HasGenericInterface(typeof(IList<>));
        var ro = type.HasGenericInterface(typeof(IReadOnlyList<>));
        var baseType = (lst ?? ro) ?? throw new ArgumentException($"Type {type} is not a List");
        TypeCode = Type.GetTypeCode(type);
        BaseType = TypeProxy.Get(baseType);
        IsReadOnly = lst == null;
        var hasEmptyConstructor = type.HasDefaultConstructor();
        HasEmptyConstructor = hasEmptyConstructor;
        //FriendlyName = (baseType == lst) ? $"List<{BaseType.FriendlyName}>" : $"ReadOnlyList<{BaseType.FriendlyName}>";


        if (isnullallowed)
            _defaultvalue = () => null;
        else
        {
            if (hasEmptyConstructor)
                _defaultvalue = () => null;
            else
            {
                var primitivetype = typeof(List<>).MakeGenericType(baseType);
                _defaultvalue = () => Activator.CreateInstance(primitivetype);
            }
        }
    }
  
}