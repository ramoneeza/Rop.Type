using System;

namespace Rop.Types.Types;

internal class TypeProxyEnum : AbsTypeProxy,ITypeProxyEnum
{
    public override ITypeProxy BaseType { get; }
    public override bool HasEmptyConstructor => true;
    protected override Func<object?> DefaultValue { get; }

    public TypeProxyEnum(Type type, bool isnullallowed):base(type,TypeType.Enum,false)
    {
        var subtype = type.GetEnumUnderlyingType();
        if (subtype is null) throw new ArgumentException($"Type {type} is not Enum");
        BaseType = TypeProxy.Get(subtype);
        DefaultValue =()=> Activator.CreateInstance(type);
    }
}

public interface ITypeProxyEnum:ITypeProxy
{
}
