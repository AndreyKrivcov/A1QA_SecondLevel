using System;
using System.Collections.ObjectModel;
using System.Linq;

using OpenQA.Selenium;
using SeleniumWrapper.Browser;

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

        private readonly WebElementKeeper element;
        public virtual bool IsExists =>
            (element != null && ((WebElementKeeper)element).IsExists);
        internal IWebElement IWebElement => Element;
        protected IWebElement Element 
        {
            get
            {
                if(!IsExists)
                {
                    element.CreateElement();
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
        public void WaitForDisplayed(TimeSpan timeout, TimeSpan? sleepInterval = null)
        {
            BrowserWait.Wait(timeout, (IBrowser x) => Displayed, sleepInterval, typeof(NoSuchElementException));
        }       
        public void WaitForExists(TimeSpan timeout, TimeSpan? sleepInterval = null)
        {
            BrowserWait.Wait(timeout, (IBrowser x) => IsExists, sleepInterval, typeof(NoSuchElementException));
        }
        public void WaitForAvailibility(TimeSpan timeout, TimeSpan? sleepInterval = null)
        {
            BrowserWait.Wait(timeout,(IBrowser) =>!Disabled, sleepInterval, typeof(NoSuchElementException));
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
