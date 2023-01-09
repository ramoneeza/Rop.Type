using System;

namespace Rop.Types.Types;

internal class TypeProxyObject : AbsTypeProxy,ITypeProxyObject
{
    public override ITypeProxy? BaseType => null;
    protected override Func<object?> DefaultValue { get; }
    public override bool HasEmptyConstructor { get; }

    protected TypeProxyObject(Type type,TypeType typetype, bool isnullallowed):base(type,typetype, isnullallowed)
    {
        var hasEmptyConstructor = type.HasDefaultConstructor();
        HasEmptyConstructor=hasEmptyConstructor;
        if (isnullallowed)
            DefaultValue = () => null;
        else
        {
            if (hasEmptyConstructor)
                DefaultValue = () => Activator.CreateInstance(Type);
            else
            {
                DefaultValue = () => null; // Can't do nothing more
            }
        }
    }
    public TypeProxyObject(Type type, bool isnullallowed):this(type,TypeType.Object, isnullallowed)
    {
    }
}

public interface ITypeProxyObject:ITypeProxy
{
}