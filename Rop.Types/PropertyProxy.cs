using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Rop.Types;

public class PropertyProxy : IPropertyProxy
{
    // Static
    private static readonly ConcurrentDictionary<PropertyInfo, IPropertyProxy> _dic = new();
    private static readonly ConcurrentDictionary<PropertyKey, IPropertyProxy?> _dicByKey = new();
    
    public static IPropertyProxy Get(PropertyInfo prop)
    {
        if (_dic.TryGetValue(prop, out var value)) return value;
        value = new PropertyProxy(prop);
        _dic[prop] = value;
        var pk=new PropertyKey(value);
        _dicByKey[pk] = value;
        return value;
    }
    public static IPropertyProxy? Get(Type type,string propertyname)
    {
        var pk = new PropertyKey(type, propertyname);
        if (_dicByKey.TryGetValue(pk, out var value)) return value;
        var prop = type.GetProperty(propertyname);
        if (prop == null)
        {
            _dicByKey[pk] = null;
            return null;
        }
        return Get(prop);
    }

    

    private record PropertyKey
    {
        public RuntimeTypeHandle Type { get; }
        public string Name { get; }

        public PropertyKey(Type type, string name)
        {
            Type = type.TypeHandle;
            Name = name;
        }
        public PropertyKey(IPropertyProxy prop) : this(prop.DeclaringClass.Type, prop.Name){}
    }

    // Instance
    private readonly List<Attribute> _attributes;
    public PropertyInfo PropertyInfo { get; }
    public string Name => PropertyInfo.Name;
    public ITypeProxy DeclaringClass { get; }
    public bool IsNullAllowed { get; }
    public ITypeProxy PropertyType { get; }
    public IReadOnlyList<Attribute> Attributes=>_attributes;
    public bool CanRead { get; }
    public bool CanWrite { get; }
    public MethodInfo? Getter { get; }
    public MethodInfo? Setter { get; }
    private Func<object,object?>? FnGetValue { get; }
    private Action<object,object?>? ASetValue { get; }
    public object? GetValue(object item)
    {
        if (FnGetValue is null) throw new Exception("No Getter");
        return FnGetValue(item);
    }
    public void SetValue(object item,object? value)
    {
        if (ASetValue is null) throw new Exception("No Setter");
        ASetValue(item,value);
    }
    
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
        FnGetValue = CreateGetter();
        ASetValue = CreateSetter();
    }

    private Action<object, object?>? CreateSetter()
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

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals(obj as IPropertyProxy);
    }

    public override int GetHashCode()
    {
        return PropertyInfo.GetHashCode();
    }
    
    public bool Equals(IPropertyProxy? other)
    {
        if (other is null) return false;
        return (this.PropertyInfo == other.PropertyInfo);
    }

    public override string ToString()
    {
        return $"{PropertyInfo.Name}:{PropertyType}";
    }
}
