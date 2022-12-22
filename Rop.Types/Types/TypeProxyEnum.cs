namespace Rop.Types.Types;

public class TypeProxyEnum : AbsTypeProxy
{
    public override ITypeProxy BaseType { get; }
    public override bool IsBasicValueType => false;
    public override bool IsArray => false;
    public override bool IsNullable => false;
    public override bool IsList => false;
    public override bool IsEnumerable => false;
    public override bool IsEnum => true;
    public override bool IsString => false;
    public override bool HasEmptyConstructor => true;
    public override TypeCode TypeCode { get; }
    public override TypeType TypeType => TypeType.Enum;
    private readonly object _defaultvalue;
    public override object? GetDefaultValue() => _defaultvalue;
    public TypeProxyEnum(Type type, bool isnullallowed):base(type,false)
    {
        var subtype = type.GetEnumUnderlyingType();
        if (subtype is null) throw new ArgumentException($"Type {type} is not Enum");
        TypeCode = Type.GetTypeCode(type);
        BaseType = TypeProxy.Get(subtype);
        _defaultvalue = Activator.CreateInstance(type)!;
    }
}