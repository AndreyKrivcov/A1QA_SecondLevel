using System;
using System.Collections.ObjectModel;
using System.Linq;

using OpenQA.Selenium;
using SeleniumWrapper.Browser;

using BrowserWait = SeleniumWrapper.Utils.BrowserWait;

namespace SeleniumWrapper.Elements
{
    public abstract class BaseElement 
    {
        protected BaseElement(WebElementKeeper element)
        {
            if(element == null)
            {
                throw new ArgumentNullException();
            }
            this.element = element;
        }

        internal readonly WebElementKeeper element;
        public virtual bool IsExists =>(element as WebElementKeeper).IsExists;
        internal IWebElement IWebElement => Element;
        protected IWebElement Element 
        {
            get
            {
                if(!IsExists)
                {
                    throw new NoSuchElementException();
                }
                return element;
            }
        }

        public string Name => Element.GetAttribute("name");
        public string Title => Element.GetAttribute("title");
        public string Type => Element.GetAttribute("type");
        public string GetSccValue(string property) => Element.GetCssValue(property);
        public System.Drawing.Point Location => Element.Location;
        public bool Selected => Element.Selected;
        public System.Drawing.Size Size => Element.Size;
        public string TagName=> Element.TagName;
        public bool Displayed => Element.Displayed;
        public string GetAttribute(string name) => Element.GetAttribute(name);
        public bool Disabled => Element.GetAttribute("disabled") == "disabled"; 
        public string InnerHTML => Element.GetAttribute("innerHTML");
        public T WaitForDisplayed<T>(TimeSpan timeout, TimeSpan? sleepInterval = null) where T : BaseElement
        {
            BrowserWait.Wait(timeout, (IBrowser x) => IsExists && Displayed, sleepInterval, typeof(NoSuchElementException));
            return this as T;
        }       
        public T WaitForExists<T>(TimeSpan timeout, TimeSpan? sleepInterval = null) where T : BaseElement
        {
            BrowserWait.Wait(timeout, (IBrowser x) => IsExists, sleepInterval, typeof(NoSuchElementException));
            return this as T;
        }

        public T FindElement<T>(By by) where T : BaseElement => new DefaultElement<T>(Element.FindElement(by) as WebElementKeeper);
        public ReadOnlyCollection<T> FindElements<T>(By by) where T : BaseElement 
        {
            var elements = Element.FindElements(by)
                .Select(x=>new DefaultElement<T>(x as WebElementKeeper));
            return elements.ToElementArray().AsReadOnly();
        }
        public void Click() => Element.Click();

    }
}
