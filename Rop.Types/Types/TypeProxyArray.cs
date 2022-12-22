namespace Rop.Types.Types;

public class TypeProxyArray : AbsTypeProxy
{
    public sealed override ITypeProxy BaseType { get; }
    public override bool IsBasicValueType => false;
    public override bool IsArray => true;
    public override bool IsNullable => false;
    public override bool IsList => false;
    public override bool IsEnumerable => true;
    public override bool IsEnum => false;
    public override bool IsString => false;
    public override bool HasEmptyConstructor => false;
    public override TypeCode TypeCode { get; }
    public override TypeType TypeType => TypeType.Array;
    private readonly object? _defaultvalue;
    public override object? GetDefaultValue() => IsNullAllowed ? null : Array.CreateInstance(BaseType.Type, 0);
    public TypeProxyArray(Type type, bool isnullallowed):base(type,isnullallowed)
    {
        if (!type.IsArray) throw new ArgumentException($"Type {type} is not Array");
        TypeCode = Type.GetTypeCode(type);
        var baseType = type.GetElementType()!;
        BaseType = TypeProxy.Get(baseType);
        _defaultvalue = isnullallowed ? null : Array.CreateInstance(baseType, 0);
    }
}