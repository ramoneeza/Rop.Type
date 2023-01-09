using System;

namespace Rop.Types.Types;

internal class TypeProxySingleGeneric : TypeProxyObject,ITypeProxySingleGeneric
{
    public override ITypeProxy? BaseType { get; }
    public Type[] GenericArguments { get; }

    public TypeProxySingleGeneric(Type type, bool isnullallowed):base(type,TypeType.SingleGeneric, isnullallowed)
    {
        var g = type.GetGenericArguments();
        if (g.Length != 1) throw new ArgumentException(nameof(type));
        BaseType = TypeProxy.Get(g[0]);
        GenericArguments = g;
    }
}
internal class TypeProxyMultiGeneric : TypeProxyObject,ITypeProxyMultiGeneric
{
    public override ITypeProxy? BaseType => null;
    public Type[] GenericArguments { get; }

    public TypeProxyMultiGeneric(Type type, bool isnullallowed):base(type,TypeType.MultiGeneric, isnullallowed)
    {
        var g = type.GetGenericArguments();
        if (g.Length <= 1) throw new ArgumentException(nameof(type));
        GenericArguments = g;
    }
}

public interface ITypeProxyMultiGeneric:ITypeProxyGeneric
{
}

public interface ITypeProxyGeneric:ITypeProxyObject
{
    public Type[] GenericArguments { get; }
}

public interface ITypeProxySingleGeneric:ITypeProxyGeneric
{
}