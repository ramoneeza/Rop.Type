namespace Rop.Types;

public enum TypeType
{
    BasicValueType,
    Enum,
    Struct,
    Nullable,
    String,
    Array,
    List,
    Dictionary,
    Enumerable,
    Object,
    SingleGeneric,
    MultiGeneric
}
[Flags]
public enum TypeFlags
{
    IsBasicValueType=1,
    IsEnum=2,
    IsStruct=4,
    IsNullable=8,
    IsString=16,
    IsArray=32,
    IsList=64,
    IsDictionary=128,
    IsEnumerable=256,
    IsObject=512,
    IsSingleGeneric=1024,
    IsGeneric=2048
};

public static class TypeTypeHelper
{
    private static readonly Dictionary<TypeType, TypeFlags> _map = new Dictionary<TypeType, TypeFlags>()
    {
        [TypeType.BasicValueType]=TypeFlags.IsBasicValueType,
        [TypeType.Enum]=TypeFlags.IsEnum,
        [TypeType.Nullable]=TypeFlags.IsNullable|TypeFlags.IsObject|TypeFlags.IsSingleGeneric| TypeFlags.IsGeneric,
        [TypeType.Struct] = TypeFlags.IsStruct,
        [TypeType.String]=TypeFlags.IsString,
        [TypeType.Array]=TypeFlags.IsArray|TypeFlags.IsEnumerable,
        [TypeType.List]=TypeFlags.IsList|TypeFlags.IsEnumerable|TypeFlags.IsObject,
        [TypeType.Dictionary]=TypeFlags.IsDictionary|TypeFlags.IsObject,
        [TypeType.Enumerable]=TypeFlags.IsEnumerable| TypeFlags.IsObject,
        [TypeType.Object]=TypeFlags.IsObject,
        [TypeType.SingleGeneric] = TypeFlags.IsSingleGeneric| TypeFlags.IsGeneric | TypeFlags.IsObject,
        [TypeType.MultiGeneric]=TypeFlags.IsGeneric | TypeFlags.IsObject
    };
    public static TypeFlags GetTypeFlags(this TypeType type) => _map[type];
    public static bool IsBasicValueType(this TypeType type) =>type==TypeType.BasicValueType;
    public static bool IsEnum(this TypeType type) => type == TypeType.Enum;
    public static bool IsStruct(this TypeType type) => type == TypeType.Struct;
    public static bool IsNullable(this TypeType type) => type == TypeType.Nullable;
    public static bool IsString(this TypeType type) => type == TypeType.String;
    public static bool IsArray(this TypeType type) => type == TypeType.Array;
    public static bool IsList(this TypeType type) => type == TypeType.List;
    public static bool IsDictionary(this TypeType type) => type == TypeType.Dictionary;
    public static bool IsBasicEnumerable(this TypeType type) => type == TypeType.Enumerable;
    public static bool IsEnumerable(this TypeType type) => _map[type].HasFlag(TypeFlags.IsEnumerable);
    public static bool IsBasicObject(this TypeType type) => type == TypeType.Object;
    public static bool IsObject(this TypeType type) => _map[type].HasFlag(TypeFlags.IsObject);
    public static bool IsBasicSimpleGeneric(this TypeType type) => type == TypeType.SingleGeneric;
    public static bool IsSimpleGeneric(this TypeType type) => _map[type].HasFlag(TypeFlags.IsSingleGeneric);
    public static bool IsBasicMultiGeneric(this TypeType type) => type == TypeType.MultiGeneric;
    public static bool IsMultiGeneric(this TypeType type) => _map[type].HasFlag(TypeFlags.IsGeneric) && !_map[type].HasFlag(TypeFlags.IsSingleGeneric);
    public static bool IsGeneric(this TypeType type) => _map[type].HasFlag(TypeFlags.IsGeneric);
    public static bool IsBasicGeneric(this TypeType type) => IsBasicSimpleGeneric(type) | IsBasicMultiGeneric(type);

    public static bool IsBasicValueType(this TypeFlags type) =>type==TypeFlags.IsBasicValueType;
    public static bool IsEnum(this TypeFlags type) => type==TypeFlags.IsEnum;
    public static bool IsStruct(this TypeFlags type) => type==TypeFlags.IsStruct;
    public static bool IsNullable(this TypeFlags type) => type.HasFlag(TypeFlags.IsNullable);
    public static bool IsString(this TypeFlags type) => type == TypeFlags.IsString;
    public static bool IsArray(this TypeFlags type) => type.HasFlag(TypeFlags.IsArray);
    public static bool IsList(this TypeFlags type) => type.HasFlag(TypeFlags.IsList);
    public static bool IsDictionary(this TypeFlags type) => type.HasFlag(TypeFlags.IsDictionary);
    public static bool IsBasicEnumerable(this TypeFlags type) => type==_map[TypeType.Enumerable];
    public static bool IsEnumerable(this TypeFlags type) => type.HasFlag(TypeFlags.IsEnumerable);
    public static bool IsBasicObject(this TypeFlags type) => type==_map[TypeType.Object];
    public static bool IsObject(this TypeFlags type) => type.HasFlag(TypeFlags.IsObject);
    public static bool IsBasicSimpleGeneric(this TypeFlags type) => type ==_map[TypeType.SingleGeneric];
    public static bool IsSimpleGeneric(this TypeFlags type) => type.HasFlag(TypeFlags.IsSingleGeneric);
    public static bool IsBasicMultiGeneric(this TypeFlags type) => type == _map[TypeType.MultiGeneric];
    public static bool IsMultiGeneric(this TypeFlags type) => type.HasFlag(TypeFlags.IsGeneric) && !type.HasFlag(TypeFlags.IsSingleGeneric);
    public static bool IsGeneric(this TypeFlags type) => type.HasFlag(TypeFlags.IsGeneric);
    public static bool IsBasicGeneric(this TypeFlags type) => IsBasicSimpleGeneric(type) | IsBasicMultiGeneric(type);
}
