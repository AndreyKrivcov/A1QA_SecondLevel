using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace SeleniumWrapper.Browser
{
    internal sealed class DriverKeeper : IWebDriver, IWrapsDriver
    {
        private DriverKeeper(Func<DriverOptions,IWebDriver> driverCreator, DriverOptions options, string version, string browserName)
        {
            this.driverCreator = driverCreator;
            this.options = options;
            this.Version = version;
            this.BrowserName = browserName;
        }
        
#region Driver
        private static IWebDriver driver;
        private readonly Func<DriverOptions,IWebDriver> driverCreator;
        private readonly DriverOptions options;
        private IWebDriver Driver
        {
            get
            {
                if(driverCreator == null)
                {
                    throw new Exception("Driver was`n instanced");
                }
                if(driver == null)
                {
                    driver = driverCreator(options);
                }
                return driver;
            }
        }
        public string Version { get; }
        public string BrowserName { get; }
        public bool IsOpened => driver != null;
#endregion
#region Instamce
        private static DriverKeeper instance;
        public static void Instance(Func<DriverOptions,IWebDriver> creator, DriverOptions options, string version, string browserName)
        {
            if(instance == null)
            {
                instance = new DriverKeeper(creator,options,version,browserName);
            }
        }
        public static DriverKeeper GetDriver => instance;
#endregion

        public void Close()
        {
            if(driver != null)
            {
                bool isLast = driver.WindowHandles.Count == 1;
                driver.Close();
                if(isLast)
                {
                    driver = null;
                }
            }
        }

        public void Quit()
        {
            if(driver != null)
            {
                driver.Quit();
                driver = null;
            }
        }

        public IOptions Manage() => Driver.Manage();

        public INavigation Navigate()=>Driver.Navigate();

        public ITargetLocator SwitchTo()=> Driver.SwitchTo();

        public IWebElement FindElement(By by)=>new WebElementKeeper(()=>Driver.FindElement(by));

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return ElementFinder.FindElements<IWebElement>(()=>Driver.FindElements(by),
            (int n)=>
            {
                return new WebElementKeeper(()=>
                {
                    var elements = Driver.FindElements(by);
                    return (n < elements.Count ? elements[n] : null);
                });
            }).AsReadOnly();
        }

        public void Dispose()
        {
            if(driver != null)
            {
                driver.Dispose();
                driver = null;
            }
        }

        public string Url 
        { 
            get => Driver.Url; 
            set => Driver.Url = value;
        }

        public string Title => Driver.Title;

        public string PageSource => Driver.PageSource;

        public string CurrentWindowHandle => Driver.CurrentWindowHandle;

        public ReadOnlyCollection<string> WindowHandles => Driver.WindowHandles;

        public IJavaScriptExecutor JavaScriptExecutor => (IJavaScriptExecutor)Driver;
        public IWebDriver WrappedDriver => Driver;
    }

    internal static class ElementFinder
    {
        public static List<T> FindElements<T>(Func<IEnumerable<IWebElement>> inputElementCreator, 
                                              Func<int,T> outputElementCreator)
        {
            var data = inputElementCreator();
            List<T> ans = new List<T>();
            for (int i = 0; i < data.Count(); i++)
            {
                ans.Add(outputElementCreator(i));
            }

            return ans;
        }
    }
}