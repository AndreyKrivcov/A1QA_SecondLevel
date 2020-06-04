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
        private readonly string popUpHealpDiv = "//div[contains(@class,\"popup2\")]";
        private readonly string popUpRegionDiv_headder = "//div[text()= \"Ваш регион\"]";
        private readonly string confermRegionButton = "//div[text()= \"Ваш регион\"]/..//div[1]/button";
#endregion
#endregion

        public ReadOnlyCollection<KeyValuePair<string,IWebElement>> PopularGoods
        {
            get
            {
                BrowserWait wait = new BrowserWait(new SystemClock() ,browser, timeout, sleep);

                var collection = wait.Until(x=>
                {
                    ReadOnlyCollection<IWebElement> ans;
                    try
                    {
                        var regionNotification = x.Window.FindElement(By.XPath(popUpRegionDiv_headder));
                        if(regionNotification.Displayed)
                        {
                            x.Window.FindElement(By.XPath(confermRegionButton)).Click();
                        }
                    }
                    catch(Exception)
                    {
                        return null;
                    }
                    finally
                    {
                        ans = x.Window.FindElements(By.XPath(popularGoodsCollection_xpath));
                    }         

                    return ans;                                      
                });
                
                List<KeyValuePair<string,IWebElement>> goods = new List<KeyValuePair<string,IWebElement>>();
                foreach (var item in collection)
                {
                    string name = item.FindElement(By.TagName(NameOfPopularGoodsCollectionsItem_tag))
                                      .Text;
                    if(!excludingPopularGoodsCollectionNames.Contains(name) &&
                       !string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name))
                    {
                        goods.Add(new KeyValuePair<string, IWebElement>(name,item));
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
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException), 
                                          typeof(StaleElementReferenceException),
                                          typeof(OpenQA.Selenium.ElementClickInterceptedException));
                //===========================================
                
                var collection = wait.Until(x=>
                {
                    try
                    {
                        x.Window.FindElement(By.XPath(allCategories_xpath)).Click();
                    }
                    catch(ElementClickInterceptedException)
                    {
                        return null;
                    }
                    
                    var data = x.Window.FindElements(By.XPath(allGoodsCollection_xpath));
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
                throw new NotImplementedException();
            }
        }

        public bool LogOut()
        {
            throw new NotImplementedException();
        }
    }
}