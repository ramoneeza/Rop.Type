using System.Reflection;

namespace Rop.Types;

public interface IPropertyProxy
{
    PropertyInfo PropertyInfo { get; }
    ITypeProxy DeclaringClass { get; }
    bool IsNullAllowed { get; }
    ITypeProxy PropertyType { get; }
    IReadOnlyList<Attribute> Attributes { get; }
    bool CanRead { get; }
    bool CanWrite { get; }
    MethodInfo? Getter { get; }
    MethodInfo? Setter { get; }
    Func<object, object?>? GetValue { get; }
    Action<object, object?>? SetValue { get; }
}