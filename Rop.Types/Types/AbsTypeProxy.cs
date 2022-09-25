using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rop.Types.Types
{
    public abstract class AbsTypeProxy:ITypeProxy,IEquatable<ITypeProxy>
    {
        public abstract Type Type { get; }
        public abstract ITypeProxy? BaseType { get; }
        public abstract bool IsNullAllowed { get; }
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
        public override int GetHashCode()
        {
            return HashCode.Combine(Type, IsNullAllowed);
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals(obj as ITypeProxy);
        }
        protected bool Equals(ITypeProxy? other)
        {
            if (other is null) return false;
            return Type == other.Type && IsNullAllowed == other.IsNullAllowed;
        }
        bool IEquatable<ITypeProxy>.Equals(ITypeProxy? other)=>Equals(other);
    }
}
