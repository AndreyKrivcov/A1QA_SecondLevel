
using System.Collections.ObjectModel;
using System.Collections.Generic;

using OpenQA.Selenium;

using WebDriverManager.DriverConfigs;
using WebDriverManager;
using System.Linq;
using System;

namespace SeleniumWrapper
{
    abstract class BrowserBase : IBrowser
    {
        public BrowserBase(IDriverConfig config, string version, string browserName, Func<IWebDriver> driverCreator)
        {
            driverManager.SetUpDriver(config,version);
            BrowserName = $"{browserName} {version}";
            this.driverCreator = driverCreator;
        }

        #region Drivers collection
        protected readonly List<IWebDriver> webDrivers = new List<IWebDriver>();

        protected readonly DriverManager driverManager = new DriverManager();

        Func<IWebDriver> driverCreator;
        #endregion

        public string BrowserName { get; }

        public ReadOnlyCollection<IBrowserWindow> Windows => 
            webDrivers.Select(x=>(IBrowserWindow)new BrowserWindow(x)).ToList().AsReadOnly();

        public void Dispose()
        {
            webDrivers.ForEach(x=>x.Dispose());
        }

        public IBrowserWindow NewWindow(string url)
        {
            IWebDriver window = OpenNewWindowTab(webDrivers.Count == 0 ? null : webDrivers.Last());
            window.Navigate().GoToUrl(url);
            return new BrowserWindow(window);
        }

        public IBrowserWindow NewWindow()
        {
            return new BrowserWindow(OpenNewWindowTab(webDrivers.Count == 0 ? null : webDrivers.Last()));
        }

        protected virtual IWebDriver OpenNewWindowTab(IWebDriver driver)
        {
            IWebDriver window;
            if(driver == null)
            {
                window = driverCreator();
            }
            else
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
                window = driver.SwitchTo().Window(driver.WindowHandles.Last());
            }

            webDrivers.Add(window);
            return window;
        }

        public void Quit()
        {
            webDrivers.ForEach(x=>x.Quit());
        }

        public IBrowserWindow SwitchToWindow(string windowHandle)
        {
            if(webDrivers.Count == 0)
            {
                throw new System.Exception("Can`t find any windows");
            }

            int ind = webDrivers.FindIndex(x=>x.CurrentWindowHandle == windowHandle);
            if(ind == -1)
            {
                throw new ArgumentException($"Can`t find window {windowHandle}");
            }
            
            return new BrowserWindow(webDrivers.First().SwitchTo().Window(windowHandle));
        }

        public void CloseWindow(string windowHandle)
        {
            int ind = webDrivers.FindIndex(x=>x.CurrentWindowHandle == windowHandle);
            if(ind == -1)
            {
                throw new ArgumentException($"Can`t find window {windowHandle}");
            }
            
            webDrivers[ind].Close();
            webDrivers.RemoveAt(ind);
        }
    }
}