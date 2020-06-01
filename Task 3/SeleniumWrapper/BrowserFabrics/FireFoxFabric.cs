using WebDriverManager.DriverConfigs.Impl;
using OpenQA.Selenium.Firefox;

namespace SeleniumWrapper.BrowserFabrics
{
    class FireFoxFabric : Fabric
    {
        public FireFoxFabric() : base(BrowserType.FireFox)
        {
        }
        public override IBrowser Create(string version) =>
            Browser.Instance(new FirefoxConfig(), version, Type.ToString(), ()=>new FirefoxDriver() );
    }
}