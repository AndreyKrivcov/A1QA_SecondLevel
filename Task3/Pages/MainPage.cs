using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumWrapper;

namespace Task_3.Pages
{
    class MainPage : PageBase
    {
        public MainPage(IBrowser browser, string pageUrl, uint timeout_s, uint sleep_mls) : base(browser,pageUrl)
        {
            timeout = TimeSpan.FromSeconds(timeout_s);
            sleep = TimeSpan.FromMilliseconds(sleep_mls); 
        }

#region Fluent wait params
        private readonly TimeSpan timeout; 
        private readonly TimeSpan sleep;
#endregion

#region Locators
#region Popular goods collection
        private readonly string popularGoodsCollection_xpath = "//div[@data-zone-name=\"category-link\"]/div/a";
        private readonly string NameOfPopularGoodsCollectionsItem_tag = "span";
        private readonly ReadOnlyCollection<string> excludingNames = new List<string>
        {
            "Скидки",
            "Мобильное приложение"
        }.AsReadOnly();
#endregion

#endregion

        public ReadOnlyCollection<KeyValuePair<string,IWebElement>> PopularGoods
        {
            get
            {
                BrowserWait browserWait = new BrowserWait(new SystemClock() ,browser, timeout, sleep);
                var collection = FindElements(popularGoodsCollection_xpath,browserWait);
                
                List<KeyValuePair<string,IWebElement>> goods = new List<KeyValuePair<string,IWebElement>>();
                foreach (var item in collection)
                {
                    string name = item.FindElement(By.TagName(NameOfPopularGoodsCollectionsItem_tag))
                                      .Text;
                    if(!excludingNames.Contains(name) &&
                       !string.IsNullOrEmpty(name) &&
                       !string.IsNullOrWhiteSpace(name))
                    {
                        goods.Add(new KeyValuePair<string, IWebElement>(name,item));
                    }
                }

                return goods.AsReadOnly();
            }
        }

        public AuthorizationPage SignIn 
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool LogOut()
        {
            throw new NotImplementedException();
        }
    }
}