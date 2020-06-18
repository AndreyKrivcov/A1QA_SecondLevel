using System;
using OpenQA.Selenium;
using WebDriverManager;
using WebDriverManager.DriverConfigs;

using SeleniumWrapper.Browser;

namespace SeleniumWrapper.BrowserFabrics
{

    public abstract class Fabric
    {
        protected Fabric(string browserName, IDriverConfig config, Func<DriverOptions,IWebDriver> driverCreator)
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
        protected readonly Func<DriverOptions,IWebDriver> driverCreator;

        public virtual IBrowser Create(string version, DriverOptions options)
        {
            new DriverManager().SetUpDriver(config,version);
            Instance(version, options);
            return new Browser.Browser();
        }
        public string BrowserName{ get; }

        protected void Instance(string version, DriverOptions options)
        {
            DriverKeeper.Instance(driverCreator,options,version,BrowserName);
        }
    }
    
}