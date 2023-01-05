namespace Rop.Types.Types;

internal class TypeProxyDictionary : AbsTypeProxy,ITypeProxyDictionary
{
    public sealed override ITypeProxy? BaseType { get; } = null;

    public ITypeProxy? DicKeyType { get; } 
    public ITypeProxy? DicValueType { get; } 

    public override bool HasEmptyConstructor { get; }
    public bool IsReadOnly { get; }
    protected override Func<object?> DefaultValue { get; }
    public TypeProxyDictionary(Type type, bool isnullallowed):base(type, TypeType.Dictionary, isnullallowed) 
    {
        var lst = type.HasGenericInterface2(typeof(IDictionary<,>));
        var ro = type.HasGenericInterface2(typeof(IReadOnlyDictionary<,>));
        var baseType = (lst ?? ro) ?? throw new ArgumentException($"Type {type} is not a List");
        DicKeyType = TypeProxy.Get(baseType.Item1);
        DicValueType = TypeProxy.Get(baseType.Item2);
        IsReadOnly = lst == null;
        var hasEmptyConstructor = type.HasDefaultConstructor();
        HasEmptyConstructor = hasEmptyConstructor;
        if (isnullallowed)
            DefaultValue = () => null;
        else
        {
            if (hasEmptyConstructor)
                DefaultValue =()=>Activator.CreateInstance(type);
            else
            {
                var primitivetype = typeof(Dictionary<,>).MakeGenericType(DicKeyType.Type,DicValueType.Type);
                DefaultValue = () => Activator.CreateInstance(primitivetype);
            }
        }
    }
}
public interface ITypeProxyDictionary:ITypeProxy
{
    ITypeProxy? DicKeyType { get; } 
    ITypeProxy? DicValueType { get; }
}
