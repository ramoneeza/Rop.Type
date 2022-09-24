using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Rop.Types;

public interface ISimpleMethodProxy
{
    MethodInfo MethodInfo { get; }
    string Name { get; }
    ITypeProxy DeclaringClass { get; }
    bool IsNullAllowed { get; }
    ITypeProxy ReturnType { get; }
    ITypeProxy ParamType { get; }
    IReadOnlyList<Attribute> Attributes { get; }
    object? GetValue(object item,object? param);
}

public class SimpleMethodProxy :ISimpleMethodProxy
{
    // Static
    private static readonly ConcurrentDictionary<MethodInfo, ISimpleMethodProxy> _dic = new();
    private static readonly ConcurrentDictionary<MethodKey, ISimpleMethodProxy?> _dicByKey = new();

    public static ISimpleMethodProxy Get(MethodInfo prop)
    {
        if (_dic.TryGetValue(prop, out var value)) return value;
        value = new SimpleMethodProxy(prop);
        _dic[prop] = value;
        var pk=new MethodKey(value);
        _dicByKey[pk] = value;
        return value;
    }
    public static ISimpleMethodProxy? Get(Type type,string methodname)
    {
        var pk = new MethodKey(type, methodname);
        if (_dicByKey.TryGetValue(pk, out var value)) return value;
        var prop = type.GetMethod(methodname);
        if (prop == null)
        {
            _dicByKey[pk] = null;
            return null;
        }
        return Get(prop);
    }
    private record MethodKey
    {
        public RuntimeTypeHandle Type { get; }
        public string Name { get; }

        public MethodKey(Type type, string name)
        {
            Type = type.TypeHandle;
            Name = name;
        }
        public MethodKey(ISimpleMethodProxy prop) : this(prop.DeclaringClass.Type, prop.Name){}
    }

    // Instance
    private readonly List<Attribute> _attributes;
    public MethodInfo MethodInfo { get; }
    public string Name => MethodInfo.Name;
    public ITypeProxy DeclaringClass { get; }
    public bool IsNullAllowed { get; }
    public ITypeProxy ReturnType { get; }
    public ITypeProxy? ParamType { get; }
    public IReadOnlyList<Attribute> Attributes=>_attributes;
    private Func<object,object?,object?>? FnGetValue { get; }
    public object? GetValue(object item,object? param)
    {
        if (FnGetValue is null) throw new Exception("No Getter");
        return FnGetValue(item,param);
    }
    private SimpleMethodProxy(MethodInfo pinfo)
    {
        MethodInfo = pinfo;
        var dc = pinfo.DeclaringType;
        if (dc is null) throw new ArgumentException("Property has not declaringtype");
        DeclaringClass =TypeProxy.Get(dc);
        _attributes = pinfo.GetCustomAttributes().ToList();
        IsNullAllowed = pinfo.CanBeNullable();
        ReturnType = TypeProxy.Get(pinfo.ReturnType, IsNullAllowed);
        var pType = pinfo.GetParameters().FirstOrDefault();
        ParamType = null;
        if (pType is not null)
        {
            var pn = pType.CanBeNullable();
            ParamType = TypeProxy.Get(pType.ParameterType, pn);
        }
        FnGetValue = CreateGetter();
    }
    private Func<object,object?, object?>? CreateGetter()
    {
        var targetType = DeclaringClass;
        var exinstance = Expression.Parameter(typeof(object), "t");
        var par0 = Expression.Parameter(typeof(object), "par0");
        var exinstancec = Expression.Convert(exinstance, targetType.Type);
        var expar =(ParamType is not null)?Expression.Convert(par0, ParamType.Type):null;
        var exMemberAccess =(expar is null)?Expression.Call(exinstancec,MethodInfo): Expression.Call(exinstancec, MethodInfo,expar);      
        var exConvertToObject = Expression.Convert(exMemberAccess, typeof(object));     // Convert(t.PropertyName, typeof(object))
        var lambda = Expression.Lambda<Func<object,object?, object?>>(exConvertToObject, exinstance,par0);
        var action = lambda.Compile();
        return action;
    }
    public override string ToString()
    {
        return $"{MethodInfo.Name}:{ReturnType}";
    }
}