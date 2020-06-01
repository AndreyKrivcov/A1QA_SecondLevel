
using System.Collections.ObjectModel;
using System.Collections.Generic;

using OpenQA.Selenium;

using WebDriverManager.DriverConfigs;
using WebDriverManager;
using System.Linq;
using System;

namespace SeleniumWrapper
{
    class Browser : IBrowser
    {
        private Browser(IDriverConfig config, string version, string browserName, Func<IWebDriver> driverCreator)
        {
            new DriverManager().SetUpDriver(config,version);
            BrowserName = $"{browserName} {version}";
            
            this.driverCreator = driverCreator;
            driver = driverCreator();

            Window = new BrowserWindow(driver);
        }

        private static Browser instance;
        public static IBrowser Instance(IDriverConfig config, string version, string browserName, Func<IWebDriver> driverCreator)
        {
            if(instance == null)
            {
                instance = new Browser(config,version,browserName,driverCreator);
            }

            return instance;
        }
        
        public string BrowserName { get; }

        private readonly Func<IWebDriver> driverCreator;

        protected IWebDriver driver;
        public ReadOnlyCollection<string> OpenedWindows => 
            (driver == null ? new List<string>().AsReadOnly() : driver.WindowHandles);

        public IBrowserWindow Window { get; private set; } 

        public event Action<IBrowser> WindowChanged;
        public event Action<IBrowser> BrowserClosed;

        private void BrowserClosedInvoke()
        {
            driver = null;
            Window = null;

            BrowserClosed?.Invoke(this);
        }
        
        public void CloseWindow(string windowHandle)
        {
            if(!OpenedWindows.Contains(windowHandle))
            {
                throw new ArgumentException($"Can`t find window with handle = {windowHandle}");
            }
            if(OpenedWindows.Count == 1)
            {
                Quit();
                return;
            }

            string currentHash = driver.CurrentWindowHandle;
            
            driver.SwitchTo().Window(windowHandle);
            WindowChanged?.Invoke(this);
            driver.Close();
            
            driver.SwitchTo().Window((currentHash == windowHandle ? OpenedWindows.Last() : currentHash));
            WindowChanged?.Invoke(this);
        }

        public void Dispose()
        {
            if(driver != null)
            {
                driver.Dispose();
                BrowserClosedInvoke();
            }
        }

        public void NewWindow(string url)
        {
            OpenNewWindowTab();
            driver.Navigate().GoToUrl(url);
            WindowChanged?.Invoke(this);
        }

        public void NewWindow()
        {
            OpenNewWindowTab();
            WindowChanged?.Invoke(this);
        }

        protected virtual void OpenNewWindowTab()
        {
            if(driver == null)
            {
                driver = driverCreator();
            }
            else
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
                driver.SwitchTo().Window(driver.WindowHandles.Last());
            }
        }

        public void Quit()
        {
            if(driver != null)
            {
                driver.Quit();
                BrowserClosedInvoke();
            }
        }

        public void SwitchToWindow(string windowHandle)
        {
            if(driver != null)
            {
                if(!OpenedWindows.Contains(windowHandle))
                {
                    throw new ArgumentException($"Can`t find window with handle = {windowHandle}");
                }

                driver.SwitchTo().Window(windowHandle);
                WindowChanged?.Invoke(this);
            }
        }
    }
}