using System.ComponentModel.DataAnnotations;

namespace Rop.Types;

public interface ITypeProxy
{
    Type Type { get; }
    public string Name => Type.Name;
    string FriendlyName { get; }
    ITypeProxy? BaseType { get; }
    bool IsNullAllowed { get; }
    bool IsBasicValueType { get; }
    bool IsArray { get; }
    bool IsNullable { get; }
    bool IsList { get; }
    bool IsEnumerable { get; }
    bool IsEnum { get; }
    bool IsString { get; }
    bool HasEmptyConstructor { get; }
    TypeCode TypeCode { get; }
    TypeType TypeType { get; }
    object? GetDefaultValue();
}