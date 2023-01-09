using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Rop.Types;

public class SimpleActionProxy :ISimpleActionProxy
{
    // Static
    private static readonly ConcurrentDictionary<MethodInfo, ISimpleActionProxy> _dic = new();
    private static readonly ConcurrentDictionary<MethodKey, ISimpleActionProxy?> _dicByKey = new();

    public static ISimpleActionProxy Get(MethodInfo prop)
    {
        if (_dic.TryGetValue(prop, out var value)) return value;
        value = new SimpleActionProxy(prop);
        _dic[prop] = value;
        var pk=new MethodKey(value);
        _dicByKey[pk] = value;
        return value;
    }
    public static ISimpleActionProxy? Get(Type type,string methodname)
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
        public MethodKey(ISimpleActionProxy prop) : this(prop.DeclaringClass.Type, prop.Name){}
    }

    // Instance
    private readonly List<Attribute> _attributes;
    public MethodInfo MethodInfo { get; }
    public string Name => MethodInfo.Name;
    public ITypeProxy DeclaringClass { get; }
    public bool IsNullAllowed { get; }
    public ITypeProxy? Param0Type { get; }
    public ITypeProxy? Param1Type { get; }
    public IReadOnlyList<Attribute> Attributes=>_attributes;
    private Action<object,object?,object?>? FnAValue { get; }
    public void Invoke(object item,object? param0=null, object? param1=null)
    {
        if (FnAValue is null) throw new Exception("No Action");
        FnAValue(item,param0,param1);
    }
    private SimpleActionProxy(MethodInfo pinfo)
    {
        MethodInfo = pinfo;
        var dc = pinfo.DeclaringType;
        if (dc is null) throw new ArgumentException("Property has not declaringtype");
        DeclaringClass =TypeProxy.Get(dc);
        _attributes = pinfo.GetCustomAttributes().ToList();
        IsNullAllowed = pinfo.CanBeNullable();
        var pars = pinfo.GetParameters();
        var pType0 = (pars.Length > 0) ? pars[0] : null;
        Param0Type = null;
        if (pType0 is not null)
        {
            var pn = pType0.CanBeNullable();
            Param0Type = TypeProxy.Get(pType0.ParameterType, pn);
        }
        var pType1 = (pars.Length > 1) ? pars[1] : null;
        Param1Type = null;
        if (pType1 is not null)
        {
            var pn = pType1.CanBeNullable();
            Param1Type = TypeProxy.Get(pType1.ParameterType, pn);
        }

        FnAValue = CreateAction();
    }
    private Action<object, object?,object?>? CreateAction()
    {
        var targetType = DeclaringClass;
        var exinstance = Expression.Parameter(typeof(object), "t");
        var par0 = Expression.Parameter(typeof(object), "par0");
        var par1 = Expression.Parameter(typeof(object), "par1");
        var exinstancec = Expression.Convert(exinstance, targetType.Type);
        var expar0 =(Param0Type is not null)?Expression.Convert(par0, Param0Type.Type):null;
        var expar1 = (Param1Type is not null) ? Expression.Convert(par1, Param1Type.Type) : null;
        var exMemberAccess =(expar0 is null && expar1 is null)?Expression.Call(exinstancec,MethodInfo): 
            ((expar1 is null)?Expression.Call(exinstancec, MethodInfo,expar0): Expression.Call(exinstancec, MethodInfo, expar0,expar1));
        var lambda = Expression.Lambda<Action<object,object?,object?>>(exMemberAccess, exinstance,par0,par1);
        var action = lambda.Compile();
        return action;
    }
    public override string ToString()
    {
        return $"{MethodInfo.Name}:void";
    }
}