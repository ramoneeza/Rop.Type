using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rop.Types.Proxies
{
    internal class ClassProxy : IClassProxy
    {
        public ITypeProxy Type { get; }
        public RuntimeTypeHandle TypeHandle => Type.Type.TypeHandle;
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, ClassProxy> _dicobject = new();
        private readonly Lazy<List<IPropertyProxy>> _properties;
        private List<IPropertyProxy> _makePublicProperties()
        {
            var lst = new List<IPropertyProxy>();
            foreach (var propertyInfo in Type.Type.GetProperties())
            {
                var p = PropertyProxy.Get(propertyInfo);
                lst.Add(p);
            }
            return lst;
        }
        private ClassProxy(Type type)
        {
            Type = TypeProxy.Get(type);
            _properties = new Lazy<List<IPropertyProxy>>(_makePublicProperties);
        }
        public IReadOnlyList<IPropertyProxy> GetPublicProperties()
        {
            var p = _properties.Value;
            return p;
        }


        public static IClassProxy Get(Type t)
        {
            if (_dicobject.TryGetValue(t.TypeHandle, out var cp)) return cp;
            if (!t.IsClass) throw new ArgumentException("Type must be a class");
            cp = new ClassProxy(t);
            _dicobject[t.TypeHandle] = cp;
            return cp;
        }
    }
}
