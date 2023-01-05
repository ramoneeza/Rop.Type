namespace Rop.Types.Types;

internal class TypeProxyList : AbsTypeProxy,ITypeProxyList
{
    public sealed override ITypeProxy BaseType { get; }
    public override bool HasEmptyConstructor { get; }
    public bool IsReadOnly { get; }
    protected override Func<object?> DefaultValue { get; }

    public TypeProxyList(Type type, bool isnullallowed):base(type, TypeType.List, isnullallowed) 
    {
        var lst = type.HasGenericInterface(typeof(IList<>));
        var ro = type.HasGenericInterface(typeof(IReadOnlyList<>));
        var baseType = (lst ?? ro) ?? throw new ArgumentException($"Type {type} is not a List");
        BaseType = TypeProxy.Get(baseType);
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
                var primitivetype = typeof(List<>).MakeGenericType(baseType);
                DefaultValue = () => Activator.CreateInstance(primitivetype);
            }
        }
    }
}


public interface ITypeProxyList:ITypeProxyEnumerable
{
    bool IsReadOnly { get; }
}