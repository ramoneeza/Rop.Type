# Rop.Types

Rop.Types gives extra information for Types and Properties

Features
========

TypeProxy
---------

Rop.Types gives extra information for Types and Properties

*TypeProxy.Get(type,isnullallowed)*

A ITypeProxy interface with extra information about types

```
public interface ITypeProxy:IEquatable<ITypeProxy>
{
     Type Type { get; }
     RuntimeTypeHandle TypeHandle { get; }
     string Name { get; }
     string FriendlyName { get; }
     TypeCode TypeCode { get; }
     TypeType TypeType { get; }
     TypeFlags TypeFlags { get; }
     ITypeProxy? BaseType { get; }
     bool IsNullAllowed { get; }
     bool HasEmptyConstructor { get; }
     object? GetDefaultValue();
     bool IsBasicValueType {get;}
     bool IsEnum {get;}
     bool IsStruct{get;}
     bool IsNullable{get;}
     bool IsString{get;}
     bool IsArray{get;}
     bool IsList{get;}
     bool IsDictionary{get;}
     bool IsBasicEnumerable{get;}
     bool IsEnumerable{get;}
     bool IsBasicObject{get;}
     bool IsObject{get;}
     bool IsBasicSimpleGeneric{get;}
     bool IsSimpleGeneric{get;}
     bool IsBasicMultiGeneric{get;}
     bool IsMultiGeneric{get;}
     bool IsGeneric{get;}
     bool IsBasicGeneric{get;}
}
```

PropertyProxy
-------------

PropertyProxy allows fast property recovery w/o reflection

*PropertyProxy.Get(Get(type,string propertyname)*

*PropertyProxy.Get(Get(tpropertyinfo)*

```
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
```

ClassProxy
----------

ClassProxy allows get all public properties (as propertyproxies) without reflection

*Get(type)*

```
public interface IClassProxy
{
    RuntimeTypeHandle TypeHandle { get; }
    ITypeProxy Type { get; }
    IReadOnlyList<IPropertyProxy> GetPublicProperties();
}
```

SimpleMethodProxy
-----------------

SimpleMethodProxy allows fast function calls w/o reflection

SimpleActionProxy
-----------------
SimpleActionProxy allows fast action calls w/o reflection

EnumerableProxy
---------------
Allows cast an Enumerable to a List or Array without knowing its type



 ------
 (C)2022 Ramón Ordiales Plaza
