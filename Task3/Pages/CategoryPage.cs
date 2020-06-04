using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumWrapper;

namespace Task_3.Pages
{
    class CategoryPageCreator
    {
        public CategoryPageCreator(IBrowser browser, IWebElement item, TimeSpan timeout_s, TimeSpan sleep_mls)
        {
            timeout = timeout_s;
            sleep = sleep_mls; 
            this.browser = browser;
            this.item = item;
        }

#region Fluent wait params
        private readonly TimeSpan timeout; 
        private readonly TimeSpan sleep;
#endregion

        private readonly IBrowser browser;
        private readonly IWebElement item;

        public CategoryPage Create()
        {
            item.Click();
            return new CategoryPage(browser,timeout,sleep);
        }
    }

    class CategoryPage : PageBase
    {   
        public CategoryPage(IBrowser browser, TimeSpan timeout_s, TimeSpan sleep_mls) : base (browser, browser.Window.Url)
        {
            timeout = timeout_s;
            sleep = sleep_mls; 
        }

#region Fluent wait params
        private readonly TimeSpan timeout; 
        private readonly TimeSpan sleep;
#endregion

        private readonly string headderSelector = "//div[@data-apiary-widget-name=\"@MarketNode/CatalogHeader\"]//h1";


        public string Headder 
        {
            get
            {
                BrowserWait wait = new BrowserWait(new SystemClock() ,browser, timeout, sleep);
                return FindElements(headderSelector, wait)[0].Text;
            }
        }

    }
}