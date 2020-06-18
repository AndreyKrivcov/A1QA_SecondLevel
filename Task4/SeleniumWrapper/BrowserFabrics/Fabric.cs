using System;
using OpenQA.Selenium;
using WebDriverManager;
using WebDriverManager.DriverConfigs;

using SeleniumWrapper.Browser;

namespace SeleniumWrapper.BrowserFabrics
{

    public abstract class Fabric
    {
        protected Fabric(string browserName, IDriverConfig config, Func<IWebDriver> driverCreator)
        {
            if(driverCreator == null)
            {
                throw new ArgumentNullException();
            }
            this.config = config;
            this.driverCreator = driverCreator;
            this.BrowserName = browserName;
        }
        
        protected readonly IDriverConfig config;
        protected readonly Func<IWebDriver> driverCreator;

        public virtual IBrowser Create(string version)
        {
            new DriverManager().SetUpDriver(config,version);
            Instance(version);
            return new Browser.Browser();
        }
        public string BrowserName{ get; }

        protected void Instance(string version)
        {
            DriverKeeper.Instance(driverCreator,version,BrowserName);
        }
    }
    
}