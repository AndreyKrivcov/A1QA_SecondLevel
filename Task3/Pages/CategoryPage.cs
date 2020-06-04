using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumWrapper;

namespace Task_3.Pages
{
    class CategoryPageCreator
    {
        public CategoryPageCreator(IBrowser browser, IWebElement item, uint timeout_s, uint sleep_mls)
        {
            timeout = timeout_s;
            sleep = sleep_mls; 
            this.browser = browser;
            this.item = item;
        }

#region Fluent wait params
        private readonly uint timeout; 
        private readonly uint sleep;
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
        public CategoryPage(IBrowser browser, uint timeout_s, uint sleep_mls) : base (browser, browser.Window.Url)
        {
            timeout = TimeSpan.FromSeconds(timeout_s);
            sleep = TimeSpan.FromMilliseconds(sleep_mls); 
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
                var headders = FindElements(headderSelector, wait);
                if(headders.Count == 0)
                {
                    throw new Exception("Can`t find htadder");
                }

                return headders[0].Text;
            }
        }

    }
}