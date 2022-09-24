using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rop.Types.Test
{
    public enum Vehicle
    {
        Car,
        Bike,
        Truck,
        Taxi,
    }
    internal class TestValues
    {
    }

    public class Usuario
    {
        public string Nombre { get; private set; } = "";
        public string Apellido { get; private set; } = "";
        public string Sexo { get; private set; } = "";

        public bool EsHombre() => Sexo == "Hombre";

        public void CambiaSexo(bool ahombre)
        {
            Sexo = (ahombre) ? "Hombre" : "Mujer";
        }

        public void CambiaNombreApellidos(string nombre, string apellidos)
        {
            Nombre = nombre;
            Apellido = apellidos;
        }

        public Usuario(string nombre, string apellido, string sexo)
        {
            Nombre = nombre;
            Apellido = apellido;
            Sexo = sexo;
        }
        public Usuario(){}
    }
}
