
using System.Collections.ObjectModel;
using System.Collections.Generic;

using OpenQA.Selenium;

using System.Linq;
using System;
using OpenQA.Selenium.Interactions;

namespace SeleniumWrapper.Browser
{
    public class Browser : IBrowser
    {
        private Browser(string version, string browserName, Func<IWebDriver> driverCreator)
        {
            this.driverCreator = driverCreator;
            BrowserName = browserName;
            Version = version;
        }
        
        private static Browser instance;
        public static IBrowser Instance(string version, string browserName, Func<IWebDriver> driverCreator)
        {
            if(instance == null)
            {
                instance = new Browser(version,browserName,driverCreator);
            }

            return instance;
        }
        
        public string BrowserName { get; }
        public string Version { get; }
        public bool IsOpened => driver != null;
        public MouseUtils MouseUtils => new MouseUtils(driver);
        public KeyUtils KeyUtils => new KeyUtils(driver);
        public IJavaScriptExecutor JavaScriptExecutor => (IJavaScriptExecutor)driver;

        private readonly Func<IWebDriver> driverCreator;

        protected IWebDriver driver;
        public ReadOnlyCollection<string> OpenedWindows => 
            (driver == null ? new List<string>().AsReadOnly() : driver.WindowHandles);


#region  Window keeper
        private IBrowserWindow myWindow;
        public IBrowserWindow Window 
        { 
            get
            {
                if(myWindow == null || driver == null)
                {
                    OpenNewWindowTab(null);
                }
                return myWindow;
            } 
        } 
#endregion

        public event Action<string> WindowChanged;
        public event Action<string> WindowClosed;
        public event Action BrowserClosed;
        public event Action BrowserOpened;

        private void BrowserClosedInvoke()
        {
            driver = null;

            BrowserClosed?.Invoke();
        }
        
        public void CloseWindow(string windowHandle)
        {
            if(driver == null)
            {
                return;
            }
            if(!OpenedWindows.Contains(windowHandle))
            {
                throw new ArgumentException($"Can`t find window with handle = {windowHandle}");
            }
            if(OpenedWindows.Count == 1)
            {
                Quit();
                return;
            }

            string currentHandle = driver.CurrentWindowHandle;
            
            bool windowChanged = false;
            if(currentHandle != windowHandle)
            {
                driver.SwitchTo().Window(windowHandle).Close();
                driver.SwitchTo().Window(currentHandle);
            }
            else
            {
                driver.Close();
                driver.SwitchTo().Window(OpenedWindows.Last());
                windowChanged = true;
            }

            WindowClosed?.Invoke(windowHandle);
            if(windowChanged)
            {
                WindowChanged?.Invoke(OpenedWindows.Last());
            }
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
            OpenNewWindowTab(url);
        }

        public void NewWindow()
        {
            OpenNewWindowTab(null);
        }

        protected virtual void OpenNewWindowTab(string url)
        {
            bool shouldOpen = driver == null;
            if(shouldOpen)
            {
                driver = driverCreator();
                myWindow = new BrowserWindow(driver);
            }
            else
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
                driver.SwitchTo().Window(driver.WindowHandles.Last());
            }

            if(!string.IsNullOrEmpty(url) || !string.IsNullOrWhiteSpace(url))
            {
                driver.Navigate().GoToUrl(url);
            }

            if(!shouldOpen)
            {
                WindowChanged?.Invoke(driver.WindowHandles.Last());
            }
            else
            {
                BrowserOpened?.Invoke();
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
                WindowChanged?.Invoke(windowHandle);
            }
        }
    }
}