using WebDriverManager.DriverConfigs.Impl;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;

namespace SeleniumWrapper.BrowserFabrics
{
    class ChromeFabric : Fabric
    {
        public ChromeFabric() : base(BrowserType.Chrome.ToString(), new ChromeConfig(), GetBrowser)
        {
        }

        private static IWebDriver GetBrowser(DriverOptions options)
        {
            if(options == null)
            {
                return new ChromeDriver();
            }
            else if(options is ChromeOptions o)
            {
                return new ChromeDriver(o);
            }
            else
            {
                throw new System.Exception("Options must have \"ChromeOptions\" type or derived");
            }
        }
    }
    class FireFoxFabric : Fabric
    {
        public FireFoxFabric() : base(BrowserType.FireFox.ToString(), new FirefoxConfig(), GetBrowser)
        {
        }

        private static IWebDriver GetBrowser(DriverOptions options)
        {
            if(options == null)
            {
                return new FirefoxDriver();
            }
            else if(options is FirefoxOptions o)
            {
                return new FirefoxDriver(o);
            }
            else
            {
                throw new System.Exception("Options must have \"FirefoxOptions\" type or derived");
            }
        }
    }

    public enum BrowserType
    {
        Chrome,
        FireFox
    }
}