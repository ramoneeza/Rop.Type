using System;

namespace Rop.Types.Types;

internal class TypeProxyArray : AbsTypeProxy,ITypeProxyArray
{
    public sealed override ITypeProxy BaseType { get; }
    public override bool HasEmptyConstructor => false;
    protected override Func<object?> DefaultValue { get; }

    public TypeProxyArray(Type type, bool isnullallowed):base(type,TypeType.Array,isnullallowed)
    {
        if (!type.IsArray) throw new ArgumentException($"Type {type} is not Array");
        var baseType = type.GetElementType()!;
        BaseType = TypeProxy.Get(baseType);
        // ReSharper disable once VirtualMemberCallInConstructor
        if (isnullallowed)
            DefaultValue = ()=>null;
        else
            DefaultValue = ()=>Array.CreateInstance(BaseType.Type, 0);
    }
}

public interface ITypeProxyArray:ITypeProxyEnumerable
{
}