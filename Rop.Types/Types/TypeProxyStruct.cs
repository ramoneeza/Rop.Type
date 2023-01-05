namespace Rop.Types.Types;

internal class TypeProxyStruct : AbsTypeProxy,ITypeProxyStruct
{
    public override ITypeProxy? BaseType => null;
    public override bool HasEmptyConstructor => true;
    protected override Func<object?> DefaultValue { get; }

    public TypeProxyStruct(Type type, bool isnullallowed):base(type,TypeType.Struct,false)
    {
        if (!type.IsValueType || type.IsPrimitive) throw new ArgumentException($"Type {type} is not Struct");
        DefaultValue = ()=>Activator.CreateInstance(type)!;
    }
}
public interface ITypeProxyStruct:ITypeProxy
{
}