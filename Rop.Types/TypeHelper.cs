using System.Collections;
using System.Reflection;
using System.Text;

namespace Rop.Types
{
    public static class TypeHelper
    {
        public static bool CanBeNullable(this PropertyInfo pinfo)
        {
            return (!pinfo.PropertyType.IsValueType && pinfo.GetCustomAttributes().Any(a => a.GetType().Name.Contains("NullableAttribute")))
                   || (Nullable.GetUnderlyingType(pinfo.PropertyType) != null);
        }
        public static bool CanBeNullable(this MethodInfo pinfo)
        {
            return (!pinfo.ReturnType.IsValueType && pinfo.GetCustomAttributes().Any(a => a.GetType().Name.Contains("NullableAttribute")))
                   || (Nullable.GetUnderlyingType(pinfo.ReturnType) != null);
        }
        public static bool CanBeNullable(this ParameterInfo pinfo)
        {
            return (!pinfo.ParameterType.IsValueType && pinfo.GetCustomAttributes().Any(a => a.GetType().Name.Contains("NullableAttribute")))
                   || (Nullable.GetUnderlyingType(pinfo.ParameterType) != null);
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

        public static string GetFriendlyName(this Type type)
        {
                if (type == typeof(int)) return "int";
                if (type == typeof(short)) return "short";
                if (type == typeof(byte)) return "byte";
                if (type == typeof(bool)) return "bool";
                if (type == typeof(long)) return "long";
                if (type == typeof(float)) return "float";
                if (type == typeof(double)) return "double";
                if (type == typeof(decimal)) return "decimal";
                if (type == typeof(string)) return "string";
                if (type.IsArray)
                {
                    var et=type.GetElementType();
                    return $"{et!.GetFriendlyName()}[]";
                }
                if (type.IsGenericType)
                {
                    var b=type.GetGenericTypeDefinition();
                    if (b == typeof(Nullable<>))
                    {
                        var bs = type.GetGenericArguments()[0];
                        return bs.GetFriendlyName() + "?";
                    }

                    var sb = new StringBuilder();
                    sb.Append(type.Name.Split('`')[0]);
                    sb.Append("<");
                    sb.Append(string.Join(',', type.GetGenericArguments().Select(GetFriendlyName)));
                    sb.Append(">");
                    return sb.ToString();
                }
                return type.Name;
        }
        public static IEnumerable<(IPropertyProxy, object?, object?)> ExtendedComparer(Type t,object obj1, object obj2)
        {
            if (!t.IsClass) throw new ArgumentException("Type must be a class");
            if (obj1.GetType() != t || obj2.GetType() != t) throw new ArgumentException("Objects must by of type T");
            var props =PropertyProxy.GetPublicProperties(t);
            foreach (var pp in props)
            {
                var v1=pp.GetValue(obj1);
                var v2=pp.GetValue(obj2);
                if (!object.Equals(v1, v2))
                {
                    yield return (pp,v1, v2);
                }
            }
        }
        public static IEnumerable<(IPropertyProxy, object?, object?)> ExtendedComparer<T>(T obj1,T obj2) where T : class
        {
            return ExtendedComparer(typeof(T), obj1, obj2);
        }
    }
}
