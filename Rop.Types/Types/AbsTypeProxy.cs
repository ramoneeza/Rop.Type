using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rop.Types.Types
{
    public abstract class AbsTypeProxy:ITypeProxy,IEquatable<ITypeProxy>
    {
        public Type Type { get; }
        public string FriendlyName { get; }
        public bool IsNullAllowed { get; }
        private readonly int _hashcode;
        public abstract ITypeProxy? BaseType { get; }
        public abstract bool IsBasicValueType { get; }
        public abstract bool IsArray { get; }
        public abstract bool IsNullable { get; }
        public abstract bool IsList { get; }
        public abstract bool IsEnumerable { get; }
        public abstract bool IsEnum { get; }
        public abstract bool IsString { get; }
        public abstract bool HasEmptyConstructor { get; }
        public abstract TypeCode TypeCode { get; }
        public abstract TypeType TypeType { get; }
        public abstract object? GetDefaultValue();
        protected AbsTypeProxy(Type type,bool isNullAllowed)
        {
            Type = type;
            FriendlyName = type.GetFriendlyName();
            IsNullAllowed = isNullAllowed;
            _hashcode=HashCode.Combine(Type, IsNullAllowed);
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
