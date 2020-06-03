using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumWrapper;
using SeleniumWrapper.BrowserFabrics;

using Task_3.Pages;

namespace Task_3
{
    public class SeleniumTest
    {
        [SetUp]
        public void Setup()
        {
            config = ConfigSerializer.Deserialize();
            if(config == null)
            {
                config = new Config();
                ConfigSerializer.Serialize(config);
            }
        }

        Config config;
        
        /*
        [Test]
        public void FluentWaitExample()
        {
            IBrowser browser = new BrowserFabric().GetBrowser(BrowserType.Chrome, "81.0.4044.138");
            string target_xpath = "//h3[.='LambdaTest: Most Powerful Cross Browser Testing Tool Online']";
            
            BrowserWait browserWait = new BrowserWait(new SystemClock(), browser, TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(250));
            browserWait.Message = "Element to be searched not found";

            browser.Window.Url = "https://www.google.com/ncr";
            browser.Window.FindElement(By.Name("q")).SendKeys("LambdaTest" + Keys.Enter);
            
            IWebElement searchedResult = browserWait.Until(x => x.Window.FindElement(By.XPath(target_xpath)));
            searchedResult.Click();
        }*/

        [Test]
        public void Test_YandexMarket()
        {
            using( IBrowser browser = new BrowserFabric().GetBrowser(config.Browser, config.BrowserVersion))
            {
                browser.Window.Maximize();
                
                MainPage mainPage = new MainPage(browser,config.Url,config.WaitSecondsTimeuot,config.WaitMilisecondsSleepage);
                var popularGoods = mainPage.PopularGoods;
            }
        }
    }
}
