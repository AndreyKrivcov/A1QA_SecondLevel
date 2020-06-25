using System;
using System.Collections.Generic;
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

    internal sealed class DefaultElement<T> : BaseElement where T : BaseElement
    {
        public DefaultElement(WebElementKeeper elementKeeper) : base(elementKeeper)
        {
        }

        public static implicit operator T(DefaultElement<T> element)
        {
            return (T)Activator.CreateInstance(typeof(T),element.Element);
        }
    }

    internal static class ElementCollectionExtention
    {
        public static List<T> ToElementArray<T>(this IEnumerable<DefaultElement<T>> collection) where T : BaseElement
        {
            List<T> ans = new List<T>();
            foreach (var item in collection)
            {
                ans.Add(item);
            }

            return ans;
        }

        public static List<T> ToElementArray<T>(this IEnumerable<IWebElement> collection, Func<int,T> outputElementCreator)
        {
            List<T> ans = new List<T>();
            for (int i = 0; i < collection.Count(); i++)
            {
                ans.Add(outputElementCreator(i));
            }

            return ans;
        }
    }
}
