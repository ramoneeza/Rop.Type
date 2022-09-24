using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace Rop.Types.Benchmark
{
    public class ProxyBenchmark
    {
        private static IPropertyProxy UsuarioNombreProxy = PropertyProxy.Get(typeof(Usuario), "Nombre")!;
        private static Usuario usuario = new Usuario() {Nombre = "Ramon", Apellido = "Perez"};

        [Benchmark]
        public void GetValue()
        {
            UsuarioNombreProxy.GetValue(usuario);
        }
        [Benchmark]
        public void GetValueByReflection()
        {
            UsuarioNombreProxy.Getter.Invoke(usuario,null);
        }
    }
}
