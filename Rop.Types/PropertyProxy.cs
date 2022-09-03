using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Rop.Types;

public class PropertyProxy : IPropertyProxy
{
    // Static
    private static readonly ConcurrentDictionary<PropertyInfo, IPropertyProxy> _dic = new();
    public static IPropertyProxy? Get(PropertyInfo prop)
    {
        if (_dic.TryGetValue(prop, out var value)) return value;
        value = new PropertyProxy(prop);
        _dic[prop] = value;
        return value;
    }
    // Instance
    private readonly List<Attribute> _attributes;
    public PropertyInfo PropertyInfo { get; }
    public ITypeProxy DeclaringClass { get; }
    public bool IsNullAllowed { get; }
    public ITypeProxy PropertyType { get; }
    public IReadOnlyList<Attribute> Attributes=>_attributes;
    public bool CanRead { get; }
    public bool CanWrite { get; }
    public MethodInfo? Getter { get; }
    public MethodInfo? Setter { get; }

    public Func<object,object?>? GetValue { get; }
    public Action<object,object?>? SetValue { get; }

    private PropertyProxy(PropertyInfo pinfo)
    {
        PropertyInfo = pinfo;
        var dc = pinfo.DeclaringType;
        if (dc is null) throw new ArgumentException("Property has not declaringtype");
        DeclaringClass =TypeProxy.Get(dc);
        _attributes = pinfo.GetCustomAttributes().ToList();
        IsNullAllowed = pinfo.CanBeNullable();
        PropertyType = TypeProxy.Get(pinfo.PropertyType, IsNullAllowed);
        CanRead=pinfo.CanRead;
        CanWrite = pinfo.CanWrite;
        Getter = pinfo.GetMethod;
        Setter = pinfo.SetMethod;
        GetValue = CreateGetter();
        SetValue = CreateSetter();
    }

    private Action<object, object?> CreateSetter()
    {
        if (Setter is null) return null;
        var targetType = DeclaringClass;
        var exinstance = Expression.Parameter(typeof(object), "t");
        var exinstancec = Expression.Convert(exinstance, targetType.Type);
        var exvalue = Expression.Parameter(typeof(object), "p");
        var exconvertedvalue = Expression.Convert(exvalue,PropertyType.Type);
        var exBody = Expression.Call(exinstancec,Setter,exconvertedvalue);
        var lambda = Expression.Lambda<Action<object, object?>>(exBody, exinstance, exvalue);
        var action = lambda.Compile();
        return action;
    }

    private Func<object, object?>? CreateGetter()
    {
        if (Getter is null) return null;
        var targetType = DeclaringClass;
        var exinstance = Expression.Parameter(typeof(object), "t");
        var exinstancec = Expression.Convert(exinstance, targetType.Type);
        var exMemberAccess = Expression.Call(exinstancec,Getter);       // t.PropertyName
        var exConvertToObject = Expression.Convert(exMemberAccess, typeof(object));     // Convert(t.PropertyName, typeof(object))
        var lambda = Expression.Lambda<Func<object, object?>>(exConvertToObject, exinstance);
        var action = lambda.Compile();
        return action;
    }

    public override string ToString()
    {
        return $"{PropertyInfo.Name}:{PropertyType}";
    }
}
