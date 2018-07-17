using Xunit;
namespace MyFirstUnitTests.TestSamples
{
    public class Fact
    {

        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }
        
        [Fact]
        public void FailingTest()
        {
            Assert.Equal(4, Add(2, 2));
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
