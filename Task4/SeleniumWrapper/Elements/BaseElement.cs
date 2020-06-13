using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                throw new ArgumentException("elementFinder delegate is null");
            }
            this.elementFinder = elementFinder;
            this.driver = driver;
        }

        private readonly Func<IWebElement> elementFinder;
        protected readonly IWebDriver driver;

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
            Wait(timeout, (IWebDriver x) => Displayed, sleepInterval, typeof(NoSuchElementException));
        }       
        public void WaitForExists(TimeSpan timeout, TimeSpan? sleepInterval = null)
        {
            Wait(timeout, (IWebDriver x) => IsExists, sleepInterval, typeof(NoSuchElementException));
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

        public BaseElement FindElement(By by) => new DefaultElement(()=>driver.FindElement(by),driver);
        public ElementsKeeper FindElements(By by) => new ElementsKeeper(driver, elementFinder, by);

        public void Click()
        {
            Element.Click();
        }

        public void DoubleClick()
        {
            Actions act = new Actions(driver);
            act.DoubleClick(Element);
        }
    }

    public class ElementsKeeper
    {
        public ElementsKeeper(IWebDriver driver, By by)
        {
            if(driver == null || by == null)
            {
                throw new ArgumentNullException();
            }

            this.driver = driver;
            this.by = by;
        }
        public ElementsKeeper(IWebDriver driver, Func<IWebElement> element, By by) : this(driver, by)
        {
            if(element == null)
            {
                throw new ArgumentNullException();
            }

            this.element = element;
        }

        private readonly IWebDriver driver;
        private readonly Func<IWebElement> element;
        private readonly By by;

        public static implicit operator ReadOnlyCollection<BaseElement>(ElementsKeeper collectionKeeper)
        {
            if(collectionKeeper.driver == null || collectionKeeper.by == null)
                return null;    

            ReadOnlyCollection<IWebElement> elementsFinder()
            {
                ISearchContext finder = (collectionKeeper.element == null ? collectionKeeper.driver : (ISearchContext)collectionKeeper.element());
                return finder.FindElements(collectionKeeper.by);
            }

            var data = elementsFinder();

            List<BaseElement> ans = new List<BaseElement>();
            for (int i = 0; i < data.Count; i++)
            {
                int n = i;
                ans.Add(new DefaultElement(()=>
                {
                    var items = elementsFinder();
                    return (items.Count > n ? items[n] : null);
                }, collectionKeeper.driver));
            }

            return ans.AsReadOnly();
        }
    }

    internal class DefaultElement : BaseElement
    {
        public DefaultElement(Func<IWebElement> elementFinder, IWebDriver driver) : base(elementFinder,driver)
        {
        }
    }
}
