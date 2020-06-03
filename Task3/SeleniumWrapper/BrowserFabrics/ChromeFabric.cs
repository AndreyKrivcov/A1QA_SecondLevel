using WebDriverManager.DriverConfigs.Impl;
using OpenQA.Selenium.Chrome;

namespace SeleniumWrapper.BrowserFabrics
{
    class ChromeFabric : Fabric
    {
        public ChromeFabric() : base(BrowserType.Chrome)
        {
        }
        public override IBrowser Create(string version) =>
            Browser.Instance(new ChromeConfig(), version, Type.ToString(), ()=>new ChromeDriver());
    }
}