using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Rop.Types.Types;

namespace Rop.Types
{
    public static class EnumerableHelper
    {
        #region private section
        private static readonly ConcurrentDictionary<RuntimeTypeHandle,Func<IEnumerable,object>> _dictoarray =new();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, Func<IEnumerable, object>> _dictolist = new();
        private static Func<IEnumerable, object> _makeenumerablefn(Type t,string method)
        {
            var ie =typeof(IEnumerable<>).MakeGenericType(t);
            var obj = Expression.Parameter(typeof(object), "obj");
            var cvt= Expression.Convert(obj, ie);
            var body = Expression.Call(typeof(Enumerable),method,new []{t},cvt);
            var lambda = Expression.Lambda<Func<IEnumerable,object>>(body,obj);
            return lambda.Compile();
        }

        private static Func<IEnumerable, object> GetCastList(Type t)
        {
            if (!_dictolist.TryGetValue(t.TypeHandle, out var te))
            {
                te = _makeenumerablefn(t, "ToList");
                _dictolist[t.TypeHandle] = te;
            }
            return te;
        }
        private static Func<IEnumerable,object> GetCastArray(Type t)
        {
            if (!_dictoarray.TryGetValue(t.TypeHandle, out var te))
            {
                te = _makeenumerablefn(t,"ToArray");
                _dictoarray[t.TypeHandle] = te;
            }
            return te;
        }
        #endregion
        // Cast an Enumerable<T> to an Array<T>
        public static object CastToArray(IEnumerable e,Type t)
        {
            var fn = GetCastArray(t);
            return fn(e);
        }
        // Cast an Enumerable<T> to a List<T>
        public static object CastToList(IEnumerable e, Type t)
        {
            var fn = GetCastList(t);
            return fn(e);
        }

        public static object CastToArray(this IEnumerable e)
        {
            var t = TypeProxy.Get(e.GetType()) as ITypeProxyEnumerable;
            if (t is null || t.BaseType is null) throw new ArgumentException($"{e.GetType()} is not IEnumerable<>");
            var fn = GetCastArray(t.BaseType.Type);
            return fn(e);
        }
// Cast an Enumerable<T> to a List<T>
        public static object CastToList(this IEnumerable e)
        {
            var t = TypeProxy.Get(e.GetType()) as ITypeProxyEnumerable;
            if (t is null || t.BaseType is null) throw new ArgumentException($"{e.GetType()} is not IEnumerable<>");
            var fn = GetCastList(t.BaseType.Type);
            return fn(e);
        }
    }
}
