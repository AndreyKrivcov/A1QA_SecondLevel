using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumWrapper;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Elements;

namespace Tests.Pages
{
    class GamesPage : BaseForm
    {
        public GamesPage(IBrowser browser, AgeVerificationData verificationData, TimeSpan timeout, Language ln, string gameType) : base(browser)
        {
            this.verificationData = verificationData;
            this.timeout = timeout;
            this.language = ln;

            Headder = WaitForElement<H2>(By.XPath(headderLocator),timeout).InnerHTML;
            Assert.True(Regex.Match(Headder, gameType, RegexOptions.IgnoreCase).Success);
        }

        private readonly AgeVerificationData verificationData;
        private readonly TimeSpan timeout;
        private readonly Language language;

        private readonly string itemLocator = "//div[@id=\"TopSellersRows\"]/a";
        private readonly string topSellersLocator = "//div[@id=\"tab_select_TopSellers\"]";
        private readonly string headderLocator = "//h2[@class=\"pageheader\"]";

        public List<GameItem> Games 
        {
            get
            {
                WaitForElement<Div>(By.XPath(topSellersLocator),timeout).Click();
                return Wait(timeout,(IBrowser b)=>
                {
                    var elem = browser.Window.FindElements<A>(By.XPath(itemLocator));
                    return (elem.Count == 0 ? null : elem);
                },null,typeof(NoSuchElementException))
                .Select(x=>new GameItem(x,browser,timeout,verificationData,language))
                .ToList();
            }
        }
        public string Headder { get; } 

    }

    class GameItem
    {
        public GameItem(A a, IBrowser browser, TimeSpan timeout, AgeVerificationData verificationData, Language ln)
        {
            this.a = a;
            href = a.Href;
            this.browser = browser;
            Discount = GetDiscount();
            Name = a.FindElement<Div>(By.XPath(itemName)).InnerHTML;
            DiscountedPrice = GetPrice();
            this.verificationData = verificationData;
            this.timeout = timeout;
            this.language = ln;
        }

        private readonly string discountPercent = ".//div[@class=\"discount_pct\"]";
        private readonly string itemName = ".//div[@class=\"tab_item_name\"]";
        private readonly string discountedPrice = ".//div[@class=\"discount_final_price\"]";

        private readonly A a;
        private readonly string href;
        private readonly IBrowser browser;
        private readonly AgeVerificationData verificationData;
        private readonly TimeSpan timeout;
        private readonly Language language;

        public SelectedGamePage Page 
        {
            get
            {
                Click();
                return new SelectedGamePage(browser,timeout,verificationData,language,Name);
            }
        }
        public string Name { get; }
        public int Discount { get; }
        public double DiscountedPrice { get; }

        private int GetDiscount()
        {
            try
            {
                var discount = a.FindElement<Div>(By.XPath(discountPercent));
                return Math.Abs(Convert.ToInt32(discount.InnerHTML.Replace("%","")));
            }
            catch(Exception)
            {
                return 0;
            }
        }
        private double GetPrice()
        {
            string s = a.FindElement<Div>(By.XPath(discountedPrice)).InnerHTML
                .Replace(@"\W","");
            Regex regex = new Regex("[^0-9]");
            s = regex.Replace(s,"");
            return (string.IsNullOrEmpty(s) ? 0 : Convert.ToDouble(s));
        }
        private void Click()
        {
            try
            {
                a.Click();
            }
            catch(Exception)
            {
                browser.Window.Url = href;
            }
        }
    }

    class SelectedGamePage : BaseForm
    {
        public SelectedGamePage(IBrowser browser, TimeSpan timeout, AgeVerificationData verificationData, Language ln, string gameName) : base(browser)
        {
            AgeVerificationPage verificationPage = new AgeVerificationPage(browser,timeout);
            if(verificationPage.IsPageOpened)
            {
                verificationPage.Day.SelectByValue(verificationData.Day.ToString());
                verificationPage.Month.SelectByValue(LocalisationKeeper.Get(verificationData.Month, ln));
                verificationPage.Year.SelectByValue(verificationData.Year.ToString());
                verificationPage.Submit();
            }

            Assert.True(Wait(timeout,(IBrowser b)=>
            {
                return b.Window.FindElement<Div>(By.XPath(programNameLocator)).InnerHTML == gameName;
            }, null, typeof(NoSuchElementException)));

            Name = gameName;
            this.timeout = timeout;
            this.language = ln;
            Url = browser.Window.Url;
        }   

        public string Name { get; }
        private readonly TimeSpan timeout;
        private readonly Language language;

        private readonly string confirmView = "//*[@id=\"app_agegate\"]//a/span[contains(text(),\"View\")]/..";
        private readonly string programNameLocator = "//div[@class=\"apphub_AppName\"]";
        private readonly string discountPercent = "//div[@class=\"game_purchase_action\"]//div[1][@class=\"discount_pct\"]";
        private readonly string discountedPrice = "//div[@class=\"game_purchase_action\"]//div[1][@class=\"discount_pct\"]/../div[@class=\"discount_prices\"]/div[@class=\"discount_final_price\"]";
    
        public int Discount 
        {
            get
            {
                var discount = WaitForElement<Div>(By.XPath(discountPercent),timeout);
                return Math.Abs(Convert.ToInt32(discount.InnerHTML.Replace("%","")));
            }
        }

        public double DiscountedPrice
        {
            get
            {
                var price = WaitForElement<Div>(By.XPath(discountedPrice),timeout).InnerHTML;
                Regex regex = new Regex("[^0-9]");
                price = regex.Replace(price,"");
                return (string.IsNullOrEmpty(price) ? 0 :Convert.ToDouble(price));
            }
        }

    }

    struct AgeVerificationData
    {
        public int Day;
        public Month Month;
        public int Year;
    }

    class AgeVerificationPage : BaseForm
    {
        public AgeVerificationPage(IBrowser browser, TimeSpan timeout) : base(browser)
        {
            this.timeout = timeout;
            Log(SeleniumWrapper.Logging.LogType.Info,$"Opened page \"{Url}\"","", 0);
        }

        private readonly TimeSpan timeout;

        public bool IsPageOpened
        {
            get
            {
                try
                {
                    bool isOpened = Day.Displayed && Month.Displayed && Year.Displayed && OpenPage.Displayed;
                    if(isOpened)
                    {
                        Log(SeleniumWrapper.Logging.LogType.Info,"Age verification page opened", "DiscountTest", 1);
                    }
                    return isOpened;
                }
                catch(Exception)
                {
                    return false;
                }
            }
        }

#region Locators
        private static readonly string dayLocator = "//select[@id=\"ageDay\"]";
        private static readonly string monthLocator = "//select[@id=\"ageMonth\"]";
        private static readonly string yearLocator = "//select[@id=\"ageYear\"]";
        private static readonly string submitLocator = "//div[@class=\"agegate_text_container btns\"]/a[1]";
#endregion


        private T Getter<T>(string xPath, ref T element) where T : BaseElement
        {
            CheckWindow();
            if(element == null)
            {
                element = WaitForElement<T>(By.XPath(xPath), timeout);
            }

            return element;
        }

        private Select day;
        public Select Day => Getter(dayLocator,ref day);
        private Select month;
        public Select Month => Getter(monthLocator,ref month);
        private Select year;
        public Select Year => Getter(yearLocator,ref year);
        private A submit;
        private A OpenPage => Getter(submitLocator,ref submit);

        public void Submit()
        {
            CheckWindow();
            OpenPage.Click();
        }

    }
}