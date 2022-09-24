using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rop.Types
{
    internal class GenericKey:IEquatable<GenericKey>
    {
        public RuntimeTypeHandle Type { get; }
        public IReadOnlyCollection<RuntimeTypeHandle> Parameters { get; }
        private readonly int _hashcode;
        public GenericKey(RuntimeTypeHandle type,params RuntimeTypeHandle[] parameters)
        {
            Type = type;
            Parameters = parameters;
            var hashCode = Type.GetHashCode();
            foreach (var runtimeTypeHandle in Parameters)
            {
                hashCode = hashCode * 17 + runtimeTypeHandle.GetHashCode();
            }
            _hashcode = hashCode;
        }

        public GenericKey(Type type, params Type[] parameters):this(type.TypeHandle,parameters.Select(p=>p.TypeHandle).ToArray())
        {
        }

        public bool Equals(GenericKey? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetHashCode() != _hashcode) return false;
            return Type.Equals(other.Type) && Parameters.SequenceEqual(other.Parameters);
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GenericKey) obj);
        }
        public override int GetHashCode() => _hashcode;
    }



    public static class GenericHelper
    {
        private static readonly ConcurrentDictionary<GenericKey, Type> _dic1 = new();
        public static Type GetGenericType(Type gentype,params Type[] pars)
        {
            var k =new GenericKey(gentype,pars);
            if (!_dic1.TryGetValue(k, out var t))
            {
                t = gentype.MakeGenericType(pars);
                _dic1[k] = t;
            }
            return t;
        }
        private class GenericKey : IEquatable<GenericKey>
        {
            public RuntimeTypeHandle Type { get; }
            public IReadOnlyCollection<RuntimeTypeHandle> Parameters { get; }
            private readonly int _hashcode;
            public GenericKey(RuntimeTypeHandle type, params RuntimeTypeHandle[] parameters)
            {
                Type = type;
                Parameters = parameters;
                var hashCode = Type.GetHashCode();
                foreach (var runtimeTypeHandle in Parameters)
                {
                    hashCode = hashCode * 17 + runtimeTypeHandle.GetHashCode();
                }
                _hashcode = hashCode;
            }

            public GenericKey(Type type, params Type[] parameters) : this(type.TypeHandle, parameters.Select(p => p.TypeHandle).ToArray())
            {
            }

            public bool Equals(GenericKey? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                if (other.GetHashCode() != _hashcode) return false;
                return Type.Equals(other.Type) && Parameters.SequenceEqual(other.Parameters);
            }
            public override bool Equals(object? obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((GenericKey)obj);
            }
            public override int GetHashCode() => _hashcode;
        }

    }
}
