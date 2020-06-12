using WebDriverManager.DriverConfigs.Impl;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;


namespace SeleniumWrapper.BrowserFabrics
{
    class ChromeFabric : Fabric
    {
        public ChromeFabric() : base(BrowserType.Chrome.ToString(), new ChromeConfig(), ()=>new ChromeDriver())
        {}
    }
    class FireFoxFabric : Fabric
    {
        public FireFoxFabric() : base(BrowserType.FireFox.ToString(), new FirefoxConfig(), ()=>new FirefoxDriver())
        {}
    }

    public enum BrowserType
    {
        Chrome,
        FireFox
    }
}