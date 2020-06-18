using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using SeleniumWrapper.Browser;

namespace SeleniumWrapper.Elements
{
     public class ElementsKeeper<T> where T : BaseElement
    {
        internal ElementsKeeper(By by, BaseElement fromElement = null)
        {
            this.by = by;
            this.fromElement = fromElement;
        }

        private readonly By by;
        private readonly BaseElement fromElement;

        public ReadOnlyCollection<T> Elements => this;

        public static implicit operator ReadOnlyCollection<T>(ElementsKeeper<T> collectionKeeper)
        {
            if(collectionKeeper.by == null)
            {
                return null;    
            }

            ISearchContext finder = (collectionKeeper.fromElement == null ? DriverKeeper.GetDriver 
                                        : (ISearchContext)collectionKeeper.fromElement.IWebElement);

            var data = finder.FindElements(collectionKeeper.by);

            List<T> ans = new List<T>();
            for (int i = 0; i < data.Count; i++)
            {
                ans.Add(new DefaultElement<T>(collectionKeeper.by, i, (finder is IWebDriver ? null : collectionKeeper.fromElement)));
            }

            
            return ans.AsReadOnly();
        }
    }
}