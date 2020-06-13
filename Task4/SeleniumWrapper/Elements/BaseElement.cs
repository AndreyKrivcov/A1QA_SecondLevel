using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWrapper.Elements
{
    public abstract class BaseElement 
    {
        public BaseElement(Func<IWebElement> elementFinder, IWebDriver driver)
        {
            if(elementFinder == null || driver == null)
            {
                throw new ArgumentException("elementFinder delegate is null");
            }
            this.elementFinder = elementFinder;
            this.driver = driver;
        }

        private readonly Func<IWebElement> elementFinder;
        private readonly IWebDriver driver;

        public bool IsExists 
        {
            get
            {
                try
                {
                    return elementFinder() != null;
                }
                catch(Exception)
                {
                    return false;
                }
            }
        } 

        protected IWebElement Element
        {
            get
            {
                var data = elementFinder();
                if(!IsExists)
                {
                    throw new NoSuchElementException("Can`t find element");
                }
                return data;
            }
        }

        public string GetSccValue(string property) => Element.GetCssValue(property);
        public System.Drawing.Point Location => Element.Location;
        public bool Selected => Element.Selected;
        public System.Drawing.Size Size => Element.Size;
        public string TagName=> Element.TagName;
        public bool Displayed => Element.Displayed;
        public bool Enabled => Element.Enabled;
        public void WaitForDisplayed(TimeSpan timeout, TimeSpan? sleepInterval = null)
        {
            Wait(timeout, (IWebDriver x) => Displayed);
        }       
        public void WaitForExists(TimeSpan timeout, TimeSpan? sleepInterval = null)
        {
            Wait(timeout, (IWebDriver x) => IsExists, sleepInterval);
        }

        protected T Wait<T>(TimeSpan timeout, Func<IWebDriver, T> f, TimeSpan? sleepInterval = null)
        {
            var wait = sleepInterval.HasValue ? new WebDriverWait(new SystemClock(),driver,timeout, sleepInterval.Value) 
                                              : new WebDriverWait(driver,timeout);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            return wait.Until(x=> f(x));
        } 
    }
}
