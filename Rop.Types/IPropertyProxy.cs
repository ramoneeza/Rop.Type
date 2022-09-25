using System.Reflection;

namespace Rop.Types;

public interface IPropertyProxy:IEquatable<IPropertyProxy>
{
    string Name { get; }
    PropertyInfo PropertyInfo { get; }
    ITypeProxy DeclaringClass { get; }
    bool IsNullAllowed { get; }
    ITypeProxy PropertyType { get; }
    IReadOnlyList<Attribute> Attributes { get; }
    bool CanRead { get; }
    bool CanWrite { get; }
    MethodInfo? Getter { get; }
    MethodInfo? Setter { get; }
    object? GetValue(object item);
    void SetValue(object item, object? value);
}