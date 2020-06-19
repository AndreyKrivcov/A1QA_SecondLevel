using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Interactions.Internal;

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
            int total = Driver.FindElements(by).Count;
            List<IWebElement> ans = new List<IWebElement>();
            for (int i = 0; i < total; i++)
            {
                int n = i;
                ans.Add(new WebElementKeeper(()=>
                {
                    var elements = Driver.FindElements(by);
                    return (n < elements.Count ? elements[n] : null);
                }));
            }

            return ans.AsReadOnly();
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

    internal sealed class WebElementKeeper : IWebElement, IWrapsElement
    {
        public WebElementKeeper(Func<IWebElement> creator)
        {
            this.creator = creator;
            this.element = creator();
        }

        private IWebElement element;
        public bool IsExists => (element = creator()) != null;
        private readonly Func<IWebElement> creator;

        private T Get<T>(Func<T> f)
        {
            try
            {
                return f();
            }
            catch(StaleElementReferenceException)
            {
                System.Threading.Thread.Sleep(5000);
                element = creator();
                return f();
            }
        }
        private void Get(Action f)
        {
            try
            {
                f();
            }
            catch(StaleElementReferenceException)
            {
                System.Threading.Thread.Sleep(5000);
                element = creator();
                f();
            }
        }

        public string TagName => Get(()=>element.TagName);

        public string Text => Get(()=>element.Text);

        public bool Enabled => Get(()=>element.Enabled);

        public bool Selected => Get(()=>element.Selected);

        public Point Location => Get(()=>element.Location);

        public Size Size => Get(()=>element.Size);

        public bool Displayed => Get(()=>element.Displayed);
        public IWebElement WrappedElement 
        {
            get
            {
                element = creator();
                if(element is WebElementKeeper elementKeeper)
                {
                    return elementKeeper.WrappedElement;
                }
                
                return element;
            }
        }

        public void Clear()=>Get(()=>element.Clear());

        public void Click()=>Get(()=>element.Click());

        public IWebElement FindElement(By by)
        {
            return new WebElementKeeper(()=>Get(()=>element.FindElement(by)));
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by) 
        {
            int total = Get(()=>element.FindElements(by)).Count;
            List<IWebElement> ans = new List<IWebElement>();
            for (int i = 0; i < total; i++)
            {
                int n = i;
                ans.Add(new WebElementKeeper(()=>
                {
                    var elements = Get(()=>element.FindElements(by));
                    return (n < elements.Count ? elements[n] : null);
                }));
            }

            return ans.AsReadOnly();
        } 

        public string GetAttribute(string attributeName)=>Get(()=>element.GetAttribute(attributeName));

        public string GetCssValue(string propertyName)=>Get(()=>element.GetCssValue(propertyName));

        public string GetProperty(string propertyName)=>Get(()=>element.GetProperty(propertyName));

        public void SendKeys(string text)=>Get(()=>element.SendKeys(text));

        public void Submit()=>Get(()=>element.Submit());
    }

}