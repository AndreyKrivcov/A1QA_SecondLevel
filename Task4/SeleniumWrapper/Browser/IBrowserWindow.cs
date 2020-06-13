using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumWrapper.Elements;

namespace SeleniumWrapper.Browser
{
    public interface IBrowserWindow : IWindow, INavigation
    {
        /// <summary>
        /// Get current url adress or set new
        /// </summary>
        /// <value>url adress</value>
        string Url { get; set; }
        /// <summary>
        /// Page title
        /// </summary>
        /// <value>Title</value>
        string Title { get; }
        /// <summary>
        /// Window unique handle
        /// </summary>
        /// <value>unique handle</value>
        string Handle { get; }
        BaseElement FindElement(By by);
        ElementsKeeper FindElements(By by);
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
            this.driver = driver;
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