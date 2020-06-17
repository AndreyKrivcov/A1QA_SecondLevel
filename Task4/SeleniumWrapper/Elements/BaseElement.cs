using System;
using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWrapper.Elements
{
    public abstract class BaseElement 
    {
        public BaseElement(Func<IWebElement> elementFinder, IWebDriver driver)
        {
            if(elementFinder == null || driver == null)
            {
                throw new ArgumentNullException();
            }
            this.elementFinder = elementFinder;
            this.driver = driver;
            actions = new Actions(driver);
        }

        protected readonly Func<IWebElement> elementFinder;
        protected readonly IWebDriver driver;
        protected readonly Actions actions;

        private Exception lastException;
        public bool IsExists 
        {
            get
            {
                try
                {
                    return (element = elementFinder()) != null;
                }
                catch(Exception e)
                {
                    lastException = e;
                    return false;
                }
            }
        } 

        internal IWebElement GetIWebElement()=>Element;

        private IWebElement element;
        protected IWebElement Element
        {
            get
            {
                if(!IsExists)
                {
                    throw lastException;
                }
                return element;
            }
        }

        protected void CheckTag(string expectedTag)
        {
            if(Element.TagName != expectedTag)
            {
                throw new Exception($"Wrong tag name. Expected \"{expectedTag}\", but current is \"{Element.TagName}\"");
            }
        }

        public string GetSccValue(string property) => Element.GetCssValue(property);
        public System.Drawing.Point Location => Element.Location;
        public bool Selected => Element.Selected;
        public System.Drawing.Size Size => Element.Size;
        public string TagName=> Element.TagName;
        public bool Displayed => Element.Displayed;
        public bool Enabled => Element.Enabled;
        public bool Disabled => Element.GetAttribute("disabled") == "disabled"; 
        public void WaitForDisplayed(TimeSpan timeout, TimeSpan? sleepInterval = null)
        {
            Wait(timeout, (IWebDriver x) => Displayed, sleepInterval, typeof(NoSuchElementException));
        }       
        public void WaitForExists(TimeSpan timeout, TimeSpan? sleepInterval = null)
        {
            Wait(timeout, (IWebDriver x) => IsExists, sleepInterval, typeof(NoSuchElementException));
        }

        public void WaitForAvailibility(TimeSpan timeout, TimeSpan? sleepInterval = null)
        {
            Wait(timeout, (IWebDriver x) => Enabled && !Disabled, sleepInterval, typeof(NoSuchElementException));
        }

        protected T Wait<T>(TimeSpan timeout, Func<IWebDriver, T> f, TimeSpan? sleepInterval = null, params Type[] ignoringExceptions )
        {
            var wait = sleepInterval.HasValue ? new WebDriverWait(new SystemClock(),driver,timeout, sleepInterval.Value) 
                                              : new WebDriverWait(driver,timeout);

            if(ignoringExceptions != null && ignoringExceptions.Count() > 0)
            {
                wait.IgnoreExceptionTypes(ignoringExceptions);
            }

            return wait.Until(x=> f(x));
        } 

        public T FindElement<T>(By by) where T : BaseElement => new DefaultElement<T>(()=>driver.FindElement(by),driver);
        public ElementsKeeper<T> FindElements<T>(By by) where T : BaseElement => new ElementsKeeper<T>(driver, elementFinder, by);
        public void Click() => Element.Click();

        public void ScrollToElement()
        {
            actions.MoveToElement(Element);
            actions.Perform();
        }
    }

    internal sealed class DefaultElement<T> : BaseElement where T : BaseElement
    {
        public DefaultElement(Func<IWebElement> elementFinder, IWebDriver driver) : base(elementFinder,driver)
        {
        }

        public static implicit operator T(DefaultElement<T> element)
        {
            return (T)Activator.CreateInstance(typeof(T),element.elementFinder, element.driver);
        }
    }

    public enum Sharpe
    {
        Circle,
        Default,
        Poly,
        Rect
    }

    public enum Align
    {
        Center,
        Left,
        Right,
        Justify,
    }
}
