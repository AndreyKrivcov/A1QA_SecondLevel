using System;
using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumWrapper.Browser;

namespace SeleniumWrapper.Elements
{
    public abstract class BaseElement 
    {
        protected BaseElement(By by, int ind, BaseElement parentElement)
        {
            if(by == null)
            {
                throw new ArgumentNullException();
            }

            By = by;
            this.ind = ind;
            this.parentElement = parentElement;
        }

        public By By { get; }
        internal readonly int ind;

        protected BaseElement parentElement;
        protected Exception lastException;
        protected IWebElement element;
        public virtual bool IsExists 
        {
            get
            {
                try
                {
                   // element = null;

                    //return Wait(TimeSpan.FromMinutes(1),(IWebDriver driver) =>
                  //  {
                        ISearchContext finder = (parentElement == null ? DriverKeeper.GetDriver : (ISearchContext)parentElement.IWebElement);
                        if(ind < 0)
                        {
                            element = finder.FindElement(By);
                        }
                        else
                        {
                            var data = finder.FindElements(By);
                            element = data[ind];
                        }
                        
                        return element !=null;
                 //   });
                }
                catch(Exception e)
                {
                    lastException = e;
                    return false;
                }
            }
        } 


        internal IWebElement IWebElement => Element;
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

        /*protected void CheckTag(string expectedTag)
        {
           // WaitForExists(TimeSpan.FromMinutes(1));
            
            if(Element.TagName != expectedTag)
            {
                throw new Exception($"Wrong tag name. Expected \"{expectedTag}\", but current is \"{Element.TagName}\"");
            }
        }*/

        public string GetSccValue(string property) => Element.GetCssValue(property);
        public System.Drawing.Point Location => Element.Location;
        public bool Selected => Element.Selected;
        public System.Drawing.Size Size => Element.Size;
        public string TagName=> Element.TagName;
        public bool Displayed => Element.Displayed;
        public bool Disabled => Element.GetAttribute("disabled") == "disabled"; 
        public string InnerHTML => Element.GetAttribute("innerHTML");
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
            Wait(timeout, (IWebDriver x) => !Disabled, sleepInterval, typeof(NoSuchElementException));
        }

        protected T Wait<T>(TimeSpan timeout, Func<IWebDriver, T> f, TimeSpan? sleepInterval = null, params Type[] ignoringExceptions )
        {
            var wait = sleepInterval.HasValue ? new WebDriverWait(new SystemClock(),DriverKeeper.GetDriver,timeout, sleepInterval.Value) 
                                              : new WebDriverWait(DriverKeeper.GetDriver,timeout);

            bool isStaleReferenceException = false;
            if(ignoringExceptions != null && ignoringExceptions.Count() > 0)
            {
                wait.IgnoreExceptionTypes(ignoringExceptions);
                isStaleReferenceException =  ignoringExceptions.Contains(typeof(StaleElementReferenceException));
            }
            if(!isStaleReferenceException)
            {
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            }

            return wait.Until(x=> f(x));
        } 

        public T FindElement<T>(By by) where T : BaseElement => new DefaultElement<T>(by,ind,this);
        public ElementsKeeper<T> FindElements<T>(By by) where T : BaseElement => new ElementsKeeper<T>(by, this);
        public void Click() => Element.Click();

    }

    internal sealed class DefaultElement<T> : BaseElement where T : BaseElement
    {
        public DefaultElement(By by, int ind, BaseElement parentElemen) : base(by,ind, parentElemen)
        {
        }

        public static implicit operator T(DefaultElement<T> element)
        {
            return (T)Activator.CreateInstance(typeof(T),element.By, element.ind, element.parentElement);
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
