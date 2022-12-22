namespace Rop.Types.Types;

public class TypeProxyObject : AbsTypeProxy
{
    public override ITypeProxy? BaseType => null;
    public override bool IsBasicValueType => false;
    public override bool IsArray => false;
    public override bool IsNullable => false;
    public override bool IsList => false;
    public override bool IsEnumerable => false;
    public override bool IsEnum => false;
    public override bool IsString => false;
    public override bool HasEmptyConstructor { get; }
    public override TypeCode TypeCode { get; }
    public override TypeType TypeType => TypeType.Object;
    private readonly Func<object?> _defaultvalue;
    public override object? GetDefaultValue() => _defaultvalue();
    
    public TypeProxyObject(Type type, bool isnullallowed):base(type, isnullallowed)
    {
        TypeCode = Type.GetTypeCode(type);
        var hasEmptyConstructor = type.HasDefaultConstructor();
        HasEmptyConstructor=hasEmptyConstructor;
        if (isnullallowed)
            _defaultvalue = () => null;
        else
        {
            if (hasEmptyConstructor)
                _defaultvalue = () => Activator.CreateInstance(Type);
            else
            {
                _defaultvalue = () => null; // Can't do nothing more
            }
        }
    }
}