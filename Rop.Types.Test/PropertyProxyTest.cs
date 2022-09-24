using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rop.Types.Test
{
    public class PropertyProxyTest
    {
        [Fact]
        public void TestBasicProperty()
        {
            var p = typeof(Usuario).GetProperty("Nombre");
            var a = PropertyProxy.Get(p);
            var b = PropertyProxy.Get(typeof(Usuario), "Nombre");
            Assert.Equal(a, b);
        }

        [Fact]
        public void TestBasicPropertiesOfProperty()
        {
            var a = PropertyProxy.Get(typeof(Usuario), "Nombre");
            Assert.NotNull(a);
            if (a is null) throw new Exception();
            Assert.True(a.CanRead);
            Assert.True(a.CanWrite);
            Assert.False(a.IsNullAllowed);
            Assert.Equal(typeof(Usuario), a.DeclaringClass.Type);
            Assert.Equal(typeof(string), a.PropertyType.Type);
        }

        [Fact]
        public void TestGetter()
        {
            var u = new Usuario("Ramon", "Perez", "Otro");
            var a = PropertyProxy.Get(typeof(Usuario), "Nombre");
            var expected = a.Getter.Invoke(u, null) as string;
            var getter = a.GetValue(u) as string;
            Assert.Equal(expected, getter);
            Assert.Equal("Ramon", getter);
        }
        [Fact]
        public void TestSetter()
        {
            var u = new Usuario("Ramon", "Perez", "Otro");
            var a = PropertyProxy.Get(typeof(Usuario), "Nombre");
            a.SetValue(u,"Ana");
            Assert.Equal("Ana",u.Nombre);
        }
        
    }
}
