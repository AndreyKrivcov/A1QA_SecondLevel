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
        private readonly ReadOnlyCollection<string> excludingPopularGoodsCollectionNames = new List<string>
        {
            "Скидки",
            "Мобильное приложение"
        }.AsReadOnly();
#endregion
#region All goods collection
        private readonly string allCategories_xpath = "//div[@data-zone-name=\"all-categories\"]/button";
        private readonly string allGoodsCollection_xpath = "//div[@data-zone-name=\"category-link\"]/button/a/span"; 
        private readonly ReadOnlyCollection<string> excludingAllGoodsCollectionNames = new List<string>
        {
            "Уцененные товары",
            "Скидки и акции"
        }.AsReadOnly();
#endregion
#region Region selector
        private readonly string popUpRegionDiv_headder = "//div[text()= \"Ваш регион\"]";
        private readonly string confermRegionButton = "//div[text()= \"Ваш регион\"]/..//div[1]/button";
#endregion
#region  Login logout
        private readonly string loginLink = "//div[@data-apiary-widget-name=\"@MarketNode/HeaderNav\"]//a";
        private readonly string mainLogoutButton = "//div[@data-apiary-widget-name=\"@MarketNode/HeaderNav\"]//div[1]/button";
        private readonly string logoutLink = "//div[@data-apiary-widget-name=\"@MarketNode/HeaderNav\"]//div[3]/a[6]";
#endregion
#endregion

        public ReadOnlyCollection<KeyValuePair<string,CategoryPageCreator>> PopularGoods
        {
            get
            {
                BrowserWait wait = new BrowserWait(new SystemClock() ,browser, timeout, sleep);

                var collection = wait.Until(x=>
                {
                    ReadOnlyCollection<IWebElement> ans;
                    try
                    {
                        if(FindElements(popUpRegionDiv_headder)[0].Displayed)
                        {
                            FindElements(confermRegionButton)[0].Click();
                        }
                    }
                    catch(Exception)
                    {
                        return null;
                    }
                    finally
                    {
                        ans = FindElements(popularGoodsCollection_xpath);
                    }         

                    return ans;                                      
                });
                
                List<KeyValuePair<string,CategoryPageCreator>> goods = new List<KeyValuePair<string,CategoryPageCreator>>();
                foreach (var item in collection)
                {
                    string name = item.FindElement(By.TagName(NameOfPopularGoodsCollectionsItem_tag))
                                      .Text;
                    if(!excludingPopularGoodsCollectionNames.Contains(name) &&
                       !string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name))
                    {
                        goods.Add(new KeyValuePair<string, CategoryPageCreator>(name,
                        new CategoryPageCreator(browser, item, timeout, sleep)));
                    }
                }

                return goods.AsReadOnly();
            }
        }
        public ReadOnlyCollection<string> AllCategories 
        {
            get
            {   
                var wait = new BrowserWait(new SystemClock() ,browser, timeout, sleep);

                // This think doesn`t works !!! But why ???
                //===========================================
                // wait.IgnoreExceptionTypes(typeof(NoSuchElementException), 
                //                           typeof(StaleElementReferenceException),
                //                           typeof(OpenQA.Selenium.ElementClickInterceptedException));
                //===========================================
                
                var collection = wait.Until(x=>
                {
                    try
                    {
                        FindElements(allCategories_xpath)[0].Click();
                    }
                    catch(ElementClickInterceptedException)
                    {
                        return null;
                    }
                    
                    var data = FindElements(allGoodsCollection_xpath);
                    return (data.Count == 0  || data.Any(x=>string.IsNullOrEmpty(x.Text) && string.IsNullOrWhiteSpace(x.Text)) ? null : data);
                });
                
                List<string> goodNames = new List<string>(); 
                foreach (var item in collection)
                {
                    string name = item.Text;
                    if(!excludingAllGoodsCollectionNames.Contains(name) &&
                       !string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name))
                    {
                        goodNames.Add(name);
                    }   
                }

                return goodNames.AsReadOnly();
            }
        }

        public AuthorizationPage SignIn 
        {
            get
            {
                BrowserWait wait = new BrowserWait(new SystemClock(),browser,timeout,sleep);
            
                wait.Until(x=>
                {
                    var elements =FindElements(loginLink);
                    if(elements.Count == 0 || 
                      (elements.Count > 0 && !elements[0].Displayed))
                    {
                        return null;
                    }
                    return elements[0];
                }).Click();

                browser.SwitchToWindow(browser.OpenedWindows.Last());
                return new AuthorizationPage(browser,timeout,sleep);
            }
        }

        public bool LogOut()
        {
            BrowserWait wait = new BrowserWait(new SystemClock(),browser,timeout,sleep);

            FindElements(mainLogoutButton,wait)[0].Click();
            FindElements(logoutLink, wait)[0].Click();

            return wait.Until(x=>
            {
                var elements =FindElements(loginLink);
                if(elements.Count == 0 || 
                  (elements.Count > 0 && !elements[0].Displayed))
                    {
                        return false;
                    }
                return true;
            });
        }
    }
}