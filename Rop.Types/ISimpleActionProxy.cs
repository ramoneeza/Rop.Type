using System.Reflection;

namespace Rop.Types;

public interface ISimpleActionProxy
{
    MethodInfo MethodInfo { get; }
    string Name { get; }
    ITypeProxy DeclaringClass { get; }
    bool IsNullAllowed { get; }
    ITypeProxy Param0Type { get; }
    IReadOnlyList<Attribute> Attributes { get; }
    void Invoke(object item, object? param0=null,object? param1=null);
}