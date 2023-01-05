using System.Collections;
using Rop.Types.Types;

namespace Rop.Types.Test
{
    public class TypeProxyTest
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
        public void TestDictionaryValue()
        {
            var a = TypeProxy.Get(typeof(Dictionary<string,int>));
            Assert.True(a.HasEmptyConstructor);
            Assert.False(a.IsArray);
            Assert.False(a.IsBasicValueType);
            Assert.False(a.IsEnum);
            Assert.False(a.IsEnumerable);
            Assert.False(a.IsList);
            Assert.True(a.IsNullAllowed);
            Assert.False(a.IsNullable);
            Assert.False(a.IsString);
            Assert.True(a.IsDictionary);
            var ad = a as ITypeProxyDictionary;
            Assert.NotNull(ad);
            
            Assert.Equal(typeof(string),ad.DicKeyType.Type);
            Assert.Equal(typeof(int),ad.DicValueType.Type);

            Assert.Null(a.BaseType);

            Assert.Equal(typeof(Dictionary<string,int>),a.Type);
            Assert.Equal(TypeCode.Object,a.TypeCode);
            Assert.Equal(null,a.GetDefaultValue());
        }


        [Fact]
        public void TestEnumValue()
        {
            var a = TypeProxy.Get(typeof(Vehicle));
            Assert.True(a.HasEmptyConstructor);
            Assert.False(a.IsArray);
            Assert.False(a.IsBasicValueType);
            Assert.True(a.IsEnum);
            Assert.False(a.IsEnumerable);
            Assert.False(a.IsList);
            Assert.False(a.IsNullAllowed);
            Assert.False(a.IsNullable);
            Assert.False(a.IsString);
            Assert.Equal(typeof(int),a.BaseType?.Type);
            Assert.Equal(typeof(Vehicle), a.Type);
            Assert.Equal("Vehicle", a.Name);
            Assert.Equal(TypeCode.Int32, a.TypeCode);
            Assert.Equal(Vehicle.Car, a.GetDefaultValue());
        }
        [Fact]
        public void TestNullableValue()
        {
            var a = TypeProxy.Get(typeof(int?));
            Assert.True(a.HasEmptyConstructor);
            Assert.False(a.IsArray);
            Assert.False(a.IsBasicValueType);
            Assert.False(a.IsEnum);
            Assert.False(a.IsEnumerable);
            Assert.False(a.IsList);
            Assert.True(a.IsNullAllowed);
            Assert.True(a.IsNullable);
            Assert.False(a.IsString);
            Assert.Equal(typeof(int), a.BaseType?.Type);
            Assert.Equal(typeof(int?), a.Type);
            Assert.Equal("Nullable`1", a.Name);
            Assert.Equal("int?", a.FriendlyName);
            Assert.Equal(TypeCode.Object, a.TypeCode);
            Assert.Null(a.GetDefaultValue());
        }
        [Fact]
        public void TestStringValue()
        {
            var a = TypeProxy.Get(typeof(string));
            Assert.True(a.HasEmptyConstructor);
            Assert.False(a.IsArray);
            Assert.False(a.IsBasicValueType);
            Assert.False(a.IsEnum);
            Assert.False(a.IsEnumerable);
            Assert.False(a.IsList);
            Assert.True(a.IsNullAllowed);
            Assert.False(a.IsNullable);
            Assert.True(a.IsString);
            Assert.Null(a.BaseType?.Type);
            Assert.Equal(typeof(string), a.Type);
            Assert.Equal("String", a.Name);
            Assert.Equal(TypeCode.String, a.TypeCode);
            Assert.Null(a.GetDefaultValue());
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
        public void TestListValue()
        {
            var a = TypeProxy.Get(typeof(List<int>));
            Assert.True(a.HasEmptyConstructor);
            Assert.False(a.IsArray);
            Assert.False(a.IsBasicValueType);
            Assert.False(a.IsEnum);
            Assert.True(a.IsEnumerable);
            Assert.True(a.IsList);
            Assert.True(a.IsNullAllowed);
            Assert.False(a.IsNullable);
            Assert.False(a.IsString);
            Assert.Equal(typeof(int), a.BaseType?.Type);
            Assert.Equal(typeof(List<int>), a.Type);
            Assert.Null(a.GetDefaultValue());
        }
        [Fact]
        public void TestEnumerableValue()
        {
            var a = TypeProxy.Get(typeof(HashSet<int>));
            Assert.True(a.HasEmptyConstructor);
            Assert.False(a.IsArray);
            Assert.False(a.IsBasicValueType);
            Assert.False(a.IsEnum);
            Assert.True(a.IsEnumerable);
            Assert.False(a.IsList);
            Assert.True(a.IsNullAllowed);
            Assert.False(a.IsNullable);
            Assert.False(a.IsString);
            Assert.Equal(typeof(int), a.BaseType?.Type);
            Assert.Equal(typeof(HashSet<int>), a.Type);
            Assert.Null(a.GetDefaultValue());
        }

        [Fact]
        public void TestGenericValue()
        {
            var a = TypeProxy.Get(typeof(EqualityComparer<int>));
            Assert.False(a.HasEmptyConstructor);
            Assert.False(a.IsArray);
            Assert.False(a.IsBasicValueType);
            Assert.False(a.IsEnum);
            Assert.False(a.IsEnumerable);
            Assert.False(a.IsList);
            Assert.True(a.IsNullAllowed);
            Assert.False(a.IsNullable);
            Assert.False(a.IsString);
            Assert.Equal(typeof(int), a.BaseType?.Type);
            Assert.Equal(typeof(EqualityComparer<int>), a.Type);
            Assert.Null(a.GetDefaultValue());
            var asg = a as ITypeProxySingleGeneric;
            Assert.NotNull(asg);
            var ag = a as ITypeProxyGeneric;
            Assert.NotNull(ag);

        }

        [Fact]
        public void TestMultiGenericValue()
        {
            var a = TypeProxy.Get(typeof(Action<string,int>));
            Assert.False(a.HasEmptyConstructor);
            Assert.False(a.IsArray);
            Assert.False(a.IsBasicValueType);
            Assert.False(a.IsEnum);
            Assert.False(a.IsEnumerable);
            Assert.False(a.IsList);
            Assert.True(a.IsNullAllowed);
            Assert.False(a.IsNullable);
            Assert.False(a.IsString);
            Assert.Null(a.BaseType);
            Assert.Equal(typeof(Action<string,int>), a.Type);
            Assert.Null(a.GetDefaultValue());
            var asg = a as ITypeProxyMultiGeneric;
            Assert.NotNull(asg);
            var ag = a as ITypeProxyGeneric;
            Assert.NotNull(ag);

            Assert.Equal(new[]{typeof(string),typeof(int)},asg.GenericArguments);

        }

        private struct MyStruct
        {
            public string a { get; }
            public string b { get; }
        }

        [Fact]
        public void TestStructValue()
        {
            var a = TypeProxy.Get(typeof(MyStruct));
            Assert.True(a.HasEmptyConstructor);
            Assert.False(a.IsArray);
            Assert.False(a.IsBasicValueType);
            Assert.False(a.IsEnum);
            Assert.False(a.IsEnumerable);
            Assert.False(a.IsList);
            Assert.False(a.IsNullAllowed);
            Assert.False(a.IsNullable);
            Assert.False(a.IsString);
            Assert.True(a.IsStruct);
            Assert.Null(a.BaseType);
            Assert.Equal(typeof(MyStruct), a.Type);
            Assert.Equal(new MyStruct(),a.GetDefaultValue());
            var ag = a as ITypeProxyStruct;
            Assert.NotNull(ag);
            
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
        [Fact]
        public void TestToArray2()
        {
            var a = new List<int>(){1, 2, 3, 4, 5};
            object final = a.CastToArray();
            Assert.IsType<int[]>(final);
            var finalcast = (int[]) final;
            Assert.Equal(a,finalcast.ToList());
        }
        [Fact]
        public void TestToList2()
        {
            var a = new List<int>() { 1, 2, 3, 4, 5 };
            object final = a.CastToList();
            Assert.IsType<List<int>>(final);
            var finalcast =(List<int>)final;
            Assert.Equal(a, finalcast.ToList());
        }
    }
}