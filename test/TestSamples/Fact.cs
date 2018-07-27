using System;
using System.Collections.Generic;
using Xunit;
namespace MyFirstUnitTests.TestSamples
{
    public class Fact
    {

        [Fact]
        public void EqualTest()
        {
            Assert.Equal(4, Add(2, 2));
        }
        
        [Fact]
        public void NotEqualTest()
        {
            Assert.NotEqual(5, Add(2, 2));
        }

        [Fact]
        public void SameTest()
        {
            Assert.Same("string", "string");
        }

        [Fact]
        public void NotSameTest()
        {
            Assert.NotSame("string1", "string2");
        }

        [Fact]
        public void ContainsTest()
        {
            var test_value = new List<int>();
            test_value.Add(10);
            test_value.Add(20);
            test_value.Add(30);
            
            
            Assert.Contains(20, test_value);
            Assert.Contains("n","FNZ",StringComparison.CurrentCultureIgnoreCase);

        }

        [Fact]
        public void DoesNotContainTest()
        {
            var test_value = new List<int>();
            test_value.Add(10);
            test_value.Add(20);
            test_value.Add(30);           
            
            Assert.DoesNotContain(5, test_value);
        }

        [Fact]
        public void IsTypeTest()
        {
           Assert.IsType<string>("文字列");
        }


        [Fact]
        public void IsNotTypeTest()
        {
           Assert.IsNotType<string>(10);
        }

        [Fact]
        public void NullTest()
        {
           Assert.Null(null);
        }

        [Fact]
        public void NotNullTest()
        {
           Assert.NotNull("文字列");
        }


        [Fact]
        public void ThrowsTest()
        {
           Assert.Throws<DivideByZeroException>(() => calc(10,0));
        }

        private double calc(int a, int b) {
            return a/b;
        } 


        [Fact]
        public void TrueTest()
        {
           Assert.True(1==1);
        }

        [Fact]
        public void FalseTest()
        {
           Assert.False(1==2);
        }


        [Fact]
        public void InRangeTest()
        {
           Assert.InRange(5, 1, 10);
        }

        [Fact]
        public void NotInRangeTest()
        {
           Assert.NotInRange(0, 1, 10);
        }


        [Fact]
        public void EmptyTest()
        {
           Assert.Empty(new List<string>());
        }

        [Fact]
        public void NotEmptyTest()
        {
          List<int> list = new List<int>();        
          list.Add(1);        
          Assert.NotEmpty(list);
        }


        [Fact]
        public void IsAssignableFromTest()
        {
          Assert.IsAssignableFrom<string>("TEST");
        }




        [Fact(Skip = "不需要跑")]
        public void FailingTestSkip()
        {
            Assert.Equal(5, Add(2, 2));
        }
        
        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
