using System.Collections;

namespace Rop.Types.Test
{
    public class UnitTypeProxy
    {
        [Fact]
        public void TestBasicValue()
        {
            var a = TypeProxy.Get(typeof(bool));
            Assert.True(a.HasEmptyConstructor);
            Assert.False(a.IsArray);
            Assert.True(a.IsBasicValueType);
            Assert.False(a.IsEnum);
            Assert.False(a.IsEnumerable);
            Assert.False(a.IsList);
            Assert.False(a.IsNullAllowed);
            Assert.False(a.IsNullable);
            Assert.False(a.IsString);
            Assert.Null(a.BaseType);
            Assert.Equal(typeof(bool),a.Type);
            Assert.Equal("Boolean",a.Name);
            Assert.Equal(TypeCode.Boolean,a.TypeCode);
            Assert.Equal(false,a.GetDefaultValue());
        }
        [Fact]
        public void TestArrayValue()
        {
            var a = TypeProxy.Get(typeof(int[]));
            Assert.False(a.HasEmptyConstructor);
            Assert.True(a.IsArray);
            Assert.False(a.IsBasicValueType);
            Assert.False(a.IsEnum);
            Assert.True(a.IsEnumerable);
            Assert.False(a.IsList);
            Assert.True(a.IsNullAllowed);
            Assert.False(a.IsNullable);
            Assert.False(a.IsString);
            Assert.Equal(typeof(int),a.BaseType?.Type);
            Assert.Equal(typeof(int[]), a.Type);
            Assert.Null(a.GetDefaultValue());
        }
        [Fact]
        public void TestToArray()
        {
            var a = new List<int>(){1, 2, 3, 4, 5};
            IEnumerable b = a;
            object final = EnumerableHelper.CastToArray(b, typeof(int));
            Assert.IsType<int[]>(final);
            var finalcast = (int[]) final;
            Assert.Equal(a,finalcast.ToList());
        }
        [Fact]
        public void TestToList()
        {
            var a = new List<int>() { 1, 2, 3, 4, 5 };
            IEnumerable b = a;
            object final = EnumerableHelper.CastToList(b, typeof(int));
            Assert.IsType<List<int>>(final);
            var finalcast =(List<int>)final;
            Assert.Equal(a, finalcast.ToList());
        }

    }
}