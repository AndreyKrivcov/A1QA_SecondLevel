using System;
using OpenQA.Selenium;
using WebDriverManager;
using WebDriverManager.DriverConfigs;

namespace SeleniumWrapper.BrowserFabrics
{

    public abstract class Fabric
    {
        protected Fabric(string browserName)
        {
            BrowserName = browserName;
        }
        protected Fabric(string browserName, IDriverConfig config, Func<IWebDriver> driverCreator) : this(browserName)
        {
            this.config = config;
            this.driverCreator = driverCreator;
        }
        
        protected readonly IDriverConfig config;
        protected readonly Func<IWebDriver> driverCreator;

        public virtual IBrowser Create(string version)
        {
            new DriverManager().SetUpDriver(config,version);
            return Browser.Instance(version, BrowserName, driverCreator);
        }
        public string BrowserName{ get; }
    }
    
}