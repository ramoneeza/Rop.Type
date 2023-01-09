using System;
using System.Collections.Generic;

namespace Rop.Types.Types;

internal class TypeProxyEnumerable : AbsTypeProxy,ITypeProxyEnumerable
{
    public override ITypeProxy BaseType { get; }
    public override bool HasEmptyConstructor { get; }
    protected override Func<object?> DefaultValue { get; }

    public TypeProxyEnumerable(Type type, bool isnullallowed):base(type,TypeType.Enumerable,isnullallowed)
    {
        var lst = type.HasGenericInterface(typeof(IEnumerable<>));
        var baseType = lst ?? throw new ArgumentException($"Type {type} is not a IEnumerable<>");
        BaseType = TypeProxy.Get(baseType);
        var hasemptyconstructor= type.HasDefaultConstructor();
        HasEmptyConstructor = hasemptyconstructor;
        if (!isnullallowed)
        {
            if (hasemptyconstructor)
                DefaultValue = () => Activator.CreateInstance(Type);
            else
            {
                var primitivetype = typeof(List<>).MakeGenericType(baseType);
                DefaultValue = () => Activator.CreateInstance(primitivetype);
            }
        }
        else
        {
            DefaultValue = () => null;
        }
    }
   
}

public interface ITypeProxyEnumerable:ITypeProxy
{
}
