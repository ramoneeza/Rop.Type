

using System;

namespace Rop.Types;

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