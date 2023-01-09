using System;
using System.Data;

namespace Rop.Types.Types;

internal class TypeProxyString : AbsTypeProxy,ITypeProxyString
{
    public override ITypeProxy? BaseType => null;
    public override bool HasEmptyConstructor => true;
    protected override Func<object?> DefaultValue { get; }

    public TypeProxyString(Type type, bool isnullallowed):base(type,TypeType.String,isnullallowed)
    {
        var typeCode = Type.GetTypeCode(type);
        if (typeCode != TypeCode.String) throw new ArgumentException($"Type {type} is not String");
        if (isnullallowed)
            DefaultValue = () => null;
        else
            DefaultValue = () => "";
    }
}

public interface ITypeProxyString:ITypeProxy
{
}