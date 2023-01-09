using System;

namespace Rop.Types.Types;

public interface ITypeProxyBasicValueType:ITypeProxy{}

internal class TypeProxyBasicValueType : AbsTypeProxy,ITypeProxyBasicValueType
{
    public override ITypeProxy? BaseType => null;
    public override bool HasEmptyConstructor => true;
    protected override Func<object?> DefaultValue { get; }

    public TypeProxyBasicValueType(Type type, bool isnullallowed):base(type,TypeType.BasicValueType,false)
    {
        DefaultValue = ()=>Activator.CreateInstance(type);
    }
}
