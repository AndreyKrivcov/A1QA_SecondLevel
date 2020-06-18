using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace SeleniumWrapper.Browser
{
    internal sealed class DriverKeeper : IWebDriver, IActionExecutor
    {
        private DriverKeeper(Func<IWebDriver> driverCreator, string version, string browserName)
        {
            this.driverCreator = driverCreator;
            this.Version = version;
            this.BrowserName = browserName;
        }
        
#region Driver
        private static IWebDriver driver;
        private readonly Func<IWebDriver> driverCreator;
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
                    driver = driverCreator();
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
        public static void Instance(Func<IWebDriver> creator, string version, string browserName)
        {
            if(instance == null)
            {
                instance = new DriverKeeper(creator,version,browserName);
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

        public IWebElement FindElement(By by)=>Driver.FindElement(by);

        public ReadOnlyCollection<IWebElement> FindElements(By by)=>Driver.FindElements(by);

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

        public bool IsActionExecutor => throw new NotImplementedException();

        private IActionExecutor ActionExecutor => (IActionExecutor)Driver;

        public void PerformActions(IList<ActionSequence> actionSequenceList)
        {
            ActionExecutor.PerformActions(actionSequenceList);
        }

        public void ResetInputState()
        {
            ActionExecutor.ResetInputState();
        }
    }
}