namespace Rop.Types.Test;

public class MethodProxyTest
{
    [Fact]
    public void TestGetter()
    {
        var u = new Usuario("Ramon", "Perez", "Hombre");
        var a = SimpleMethodProxy.Get(typeof(Usuario),"EsHombre");
        Assert.NotNull(a);
        var getter = (bool)a.GetValue(u,null);
        Assert.True(getter);
        
    }
    [Fact]
    public void TestSetter()
    {
        var u = new Usuario("Ramon", "Perez", "Hombre");
        var a = SimpleActionProxy.Get(typeof(Usuario), "CambiaSexo");
        Assert.NotNull(a);
        a.Invoke(u,false);
        Assert.False(u.EsHombre());

    }
    [Fact]
    public void TestSetter2()
    {
        var u = new Usuario("Ramon", "Perez", "Hombre");
        var a = SimpleActionProxy.Get(typeof(Usuario), "CambiaNombreApellidos");
        Assert.NotNull(a);
        a.Invoke(u, "Ana","Pardo");
        Assert.Equal("Ana",u.Nombre);
        Assert.Equal("Pardo", u.Apellido);

    }
}