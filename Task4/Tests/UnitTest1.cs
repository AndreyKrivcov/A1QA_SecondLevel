using NUnit.Framework;
using SeleniumWrapper.BrowserFabrics;
using SeleniumWrapper;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            IBrowser b = new BrowserFabric().GetBrowser(BrowserType.Chrome);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}