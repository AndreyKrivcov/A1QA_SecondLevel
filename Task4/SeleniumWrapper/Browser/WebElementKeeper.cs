using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using OpenQA.Selenium;

namespace SeleniumWrapper.Browser
{
    public sealed class WebElementKeeper : IWebElement, IWrapsElement
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
                CreateElement();
                return f();
            }
        }
        public void CreateElement()
        {
            element = creator();
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
                CreateElement();
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
                CreateElement();
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
            return ElementFinder.FindElements(()=>Get(()=>element.FindElements(by)),
            (int n)=>
            {
                IWebElement elementGetter()
                {
                    var elements = Get(()=>element.FindElements(by));
                    return (n < elements.Count ? elements[n] : null);
                };

                return ((element is WebElementKeeper) ? elementGetter() : new WebElementKeeper(elementGetter));
                
            }).AsReadOnly();
        } 

        public string GetAttribute(string attributeName)=>Get(()=>element.GetAttribute(attributeName));

        public string GetCssValue(string propertyName)=>Get(()=>element.GetCssValue(propertyName));

        public string GetProperty(string propertyName)=>Get(()=>element.GetProperty(propertyName));

        public void SendKeys(string text)=>Get(()=>element.SendKeys(text));

        public void Submit()=>Get(()=>element.Submit());
    }
}