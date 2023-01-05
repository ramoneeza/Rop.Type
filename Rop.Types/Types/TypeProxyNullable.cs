namespace Rop.Types.Types;

internal class TypeProxyNullable : AbsTypeProxy,ITypeProxyNullable
{
    public sealed override ITypeProxy BaseType { get; }
    public override bool HasEmptyConstructor => true;
    protected override Func<object?> DefaultValue => () => null;

    public TypeProxyNullable(Type type, bool isnullallowed):base(type,TypeType.Nullable,true)
    {
        var subtype = Nullable.GetUnderlyingType(type);
        if (subtype is null) throw new ArgumentException($"Type {type} is not Nullable<>");
        BaseType =TypeProxy.Get(subtype);
    }
    public override string ToString() => $"{FriendlyName}";
}

public interface ITypeProxyNullable:ITypeProxy
{
}

