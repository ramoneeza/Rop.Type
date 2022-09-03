using System.Collections;
using System.Reflection;

namespace Rop.Types
{
    public static class TypeHelper
    {
        public static bool CanBeNullable(this PropertyInfo pinfo)
        {
            return (!pinfo.PropertyType.IsValueType && pinfo.GetCustomAttributes().Any(a => a.GetType().Name.Contains("NullableAttribute")))
                   || (Nullable.GetUnderlyingType(pinfo.PropertyType) != null);
        }
        public static object? GetDefaultValue(this Type t)
        {
            if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
                return Activator.CreateInstance(t);
            else
                return null;
        }

        public static bool HasDefaultConstructor(this Type t)
        {
            return t.IsValueType || t.GetConstructor(Type.EmptyTypes)!=null;
        }
        public static object? TryGetNotNullDefaultValue(this Type t)
        {
            if (!t.HasDefaultConstructor()) return null;
            return Activator.CreateInstance(t);
        }


        public static Type? HasGenericInterface(this Type type, Type genericinterface)
        {
            var ie=type.GetInterfaces()
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericinterface);
            return ie?.GetGenericArguments()[0];
        }

        public static bool IsIEnumerableOfT(this Type type,out Type? subtype)
        {
            subtype = type.HasGenericInterface(typeof(IEnumerable<>));
            return subtype != null;
        }
        public static bool IsIListOfT(this Type type, out Type? subtype)
        {
            subtype = type.HasGenericInterface(typeof(IList<>));
            return subtype != null;
        }
        public static bool ImplementsGenericInterface(this Type type, Type interfaceType)
        {
            return type
                .GetTypeInfo()
                .ImplementedInterfaces
                .Any(x => x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == interfaceType);
        }

    }
}
