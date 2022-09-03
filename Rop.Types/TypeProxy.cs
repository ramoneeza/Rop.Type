using System.Collections.Concurrent;
using System.Diagnostics;
using Rop.Types.Types;

namespace Rop.Types
{
    public static class TypeProxy
    {
        private static readonly ConcurrentDictionary<(RuntimeTypeHandle,bool), ITypeProxy> _dic = new();

        internal static ITypeProxy Make(Type type, bool isnullallowed = true)
        {
            // Check Nullable
            if (Nullable.GetUnderlyingType(type) != null) return new TypeProxyNullable(type, isnullallowed);
            if (type.IsEnum) return new TypeProxyEnum(type,isnullallowed);
            if (type.IsValueType) return new TypeProxyBasicValue(type,isnullallowed);
            var typecode = Type.GetTypeCode(type);
            if (typecode == TypeCode.String) return new TypeProxyString(type, isnullallowed);
            if (type.IsArray) return new TypeProxyArray(type, isnullallowed);
            if (type.ImplementsGenericInterface(typeof(IReadOnlyList<>))||
                type.ImplementsGenericInterface(typeof(IList<>))) return new TypeProxyList(type, isnullallowed);
            if (type.ImplementsGenericInterface(typeof(IEnumerable<>))) return new TypeProxyEnumerable(type, isnullallowed);
            if (typecode != TypeCode.Object) throw new ArgumentException($"TypeType of {type} is unknown");
            return new TypeProxyObject(type, isnullallowed);
        }
        public static ITypeProxy Get(Type type,bool isnullallowed=true)
        {
            var key = (type.TypeHandle, isnullallowed);
            if (_dic.TryGetValue(key, out var item)) return item;
            item = Make(type, isnullallowed);
            _dic[key] = item;
            return item;
        }
    }
}
