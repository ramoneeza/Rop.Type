using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using Rop.Types.Types;

namespace Rop.Types
{
    public static class TypeProxy
    {
        private static readonly ConcurrentDictionary<(RuntimeTypeHandle,bool), ITypeProxy> _dic = new();

        private static ITypeProxy Make(Type type, bool isnullallowed = true)
        {
            var typecode = Type.GetTypeCode(type);

            if (typecode != TypeCode.Object)
            {
                if (type.IsEnum) return new TypeProxyEnum(type,isnullallowed);
                if (type.IsValueType) return new TypeProxyBasicValueType(type,isnullallowed);
                if (typecode == TypeCode.String) return new TypeProxyString(type, isnullallowed);
                throw new ArgumentException($"TypeType of {type} is unknown");
            }
            else
            {
                if (type.IsArray) return new TypeProxyArray(type, isnullallowed);
                if (Nullable.GetUnderlyingType(type) != null) return new TypeProxyNullable(type, isnullallowed);
                
                if (type.IsValueType) return new TypeProxyStruct(type, false);
         
                if (type.ImplementsGenericInterface(typeof(IReadOnlyList<>)) ||
                    type.ImplementsGenericInterface(typeof(IList<>)))
                    return new TypeProxyList(type, isnullallowed);
                if (type.ImplementsGenericInterface(typeof(IReadOnlyDictionary<,>)) ||
                    type.ImplementsGenericInterface(typeof(IDictionary<,>)))
                    return new TypeProxyDictionary(type, isnullallowed);
                if (type.ImplementsGenericInterface(typeof(IEnumerable<>)))
                    return new TypeProxyEnumerable(type, isnullallowed);
                if (type.IsGenericType)
                {
                    var n = type.GetGenericArguments();
                    if (n.Length==1) 
                        return new TypeProxySingleGeneric(type,isnullallowed);
                    else
                        return new TypeProxyMultiGeneric(type,isnullallowed);
                }
                return new TypeProxyObject(type, isnullallowed);
            }
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
