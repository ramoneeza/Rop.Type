using System;
using System.Collections.Generic;
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

    T? FindAtt<T>(Func<T, bool> func) where T : Attribute;
    T? FindAtt<T>() where T : Attribute;
    bool HasAtt<T>(out T? o) where T : Attribute;
}