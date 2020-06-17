using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace SeleniumWrapper.Elements
{
     public class ElementsKeeper<T> where T : BaseElement
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

        public ReadOnlyCollection<T> Elements => this;

        public static implicit operator ReadOnlyCollection<T>(ElementsKeeper<T> collectionKeeper)
        {
            if(collectionKeeper.driver == null || collectionKeeper.by == null)
                return null;    

            ReadOnlyCollection<IWebElement> elementsFinder()
            {
                ISearchContext finder = (collectionKeeper.element == null ? collectionKeeper.driver : (ISearchContext)collectionKeeper.element());
                return finder.FindElements(collectionKeeper.by);
            }

            var data = elementsFinder();

            List<T> ans = new List<T>();
            for (int i = 0; i < data.Count; i++)
            {
                int n = i;
                ans.Add(new DefaultElement<T>(()=>
                {
                    var items = elementsFinder();
                    return (items.Count > n ? items[n] : null);
                }, collectionKeeper.driver));
            }

            return ans.AsReadOnly();
        }
    }
}