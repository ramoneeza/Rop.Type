using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rop.Types.Types
{
    internal abstract class AbsTypeProxy:ITypeProxy
    {
        private readonly int _hashcode;
        public string FriendlyName { get; }
        public bool IsNullAllowed { get; }
        public Type Type { get; }
        public TypeCode TypeCode { get; }
        public TypeType TypeType { get; }
        public TypeFlags TypeFlags { get; }
        public RuntimeTypeHandle TypeHandle => Type.TypeHandle;
        public string Name => Type.Name;
        public abstract ITypeProxy? BaseType { get; }
        public virtual bool IsBasicValueType {get;}
        public virtual bool IsEnum {get;}
        public virtual bool IsStruct{get;}
        public virtual bool IsNullable{get;}
        public virtual bool IsString{get;}
        public virtual bool IsArray{get;}
        public virtual bool IsList{get;}
        public virtual bool IsDictionary{get;}
        public virtual bool IsBasicEnumerable{get;}
        public virtual bool IsEnumerable{get;}
        public virtual bool IsBasicObject{get;}
        public virtual bool IsObject{get;}
        public virtual bool IsBasicSimpleGeneric{get;}
        public virtual bool IsSimpleGeneric{get;}
        public virtual bool IsBasicMultiGeneric{get;}
        public virtual bool IsMultiGeneric{get;}
        public virtual bool IsGeneric{get;}
        public virtual bool IsBasicGeneric{get;}
        protected abstract Func<object?> DefaultValue { get; }
        public abstract bool HasEmptyConstructor { get; }
        public object? GetDefaultValue() => DefaultValue();
        protected AbsTypeProxy(Type type,TypeType typetype,TypeFlags typeFlags, bool isNullAllowed)
        {
            Type = type;
            TypeType=typetype;
            TypeFlags=typeFlags;
            IsNullAllowed = isNullAllowed;
            TypeCode = Type.GetTypeCode(type);
            FriendlyName = type.GetFriendlyName();
            #if NET6_0
            _hashcode=HashCode.Combine(Type, IsNullAllowed);
#else
            _hashcode=Type.GetHashCode()*2+(IsNullAllowed?1:0);
            #endif

            IsBasicValueType = typetype.IsBasicValueType();
            IsEnum = typetype.IsEnum();
            IsStruct=typetype.IsStruct();
            IsNullable=typetype.IsNullable();
            IsString=typetype.IsString();
            IsArray=typetype.IsArray();
            IsList=typetype.IsList();
            IsDictionary=typetype.IsDictionary();
            IsBasicEnumerable=typetype.IsBasicEnumerable();
            IsEnumerable=typetype.IsEnumerable();
            IsBasicObject=typetype.IsBasicObject();
            IsObject=typetype.IsObject();
            IsBasicSimpleGeneric=typetype.IsBasicSimpleGeneric();
            IsSimpleGeneric=typetype.IsSimpleGeneric();
            IsBasicMultiGeneric=typetype.IsBasicMultiGeneric();
            IsMultiGeneric=typetype.IsMultiGeneric();
            IsGeneric=typetype.IsGeneric();
            IsBasicGeneric=typetype.IsGeneric();

        }

        protected AbsTypeProxy(Type type,TypeType typetype, bool isNullAllowed):this(type,typetype,typetype.GetTypeFlags(),isNullAllowed)
        {
        }

        public override int GetHashCode() => _hashcode;
        public override bool Equals(object? obj)=>Equals(obj as ITypeProxy);
        protected bool Equals(ITypeProxy? other)
        {
            if (other is null) return false;
            return Type == other.Type && IsNullAllowed == other.IsNullAllowed;
        }
        bool IEquatable<ITypeProxy>.Equals(ITypeProxy? other)=>Equals(other);
        public override string ToString() => $"{FriendlyName}{(IsNullAllowed ? "?" : "")}";
    }
}
