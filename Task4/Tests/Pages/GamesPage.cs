using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumWrapper;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Elements;
using SeleniumWrapper.Utils;

namespace Tests.Pages
{
    class GamesPage : BaseForm
    {
        public GamesPage(IBrowser browser, AgeVerificationData verificationData, TimeSpan timeout, Language ln, string gameType) : base()
        {
            this.verificationData = verificationData;
            this.timeout = timeout;
            this.language = ln;

            var elem = settings.Browser.Window.FindElement<Text>(By.XPath(headderLocator)).WaitForExists<Text>(timeout);
            Headder = elem.InnerHTML;
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
                var div = settings.Browser.Window.FindElement<Contaner>(By.XPath(topSellersLocator)).WaitForExists<Contaner>(timeout);
                div.Click();
                return BrowserWait.Wait(timeout,b=>
                {
                    var elem = settings.Browser.Window.FindElements<Link>(By.XPath(itemLocator));
                    return (elem.Count == 0 ? null : elem);
                },null,typeof(NoSuchElementException))
                .Select(x=>new GameItem(x,settings.Browser,timeout,verificationData,language))
                .ToList();
            }
        }
        public string Headder { get; } 

    }

    class GameItem
    {
        public GameItem(Link a, IBrowser browser, TimeSpan timeout, AgeVerificationData verificationData, Language ln)
        {
            this.timeout = timeout;
            this.a = a;
            href = a.Href;
            this.browser = browser;
            Discount = GetDiscount();
            Name = a.FindElement<Contaner>(By.XPath(itemName)).InnerHTML;
            DiscountedPrice = GetPrice();
            this.verificationData = verificationData;
            this.language = ln;
        }

        private readonly string discountPercent = ".//div[@class=\"discount_pct\"]";
        private readonly string itemName = ".//div[@class=\"tab_item_name\"]";
        private readonly string discountedPrice = ".//div[@class=\"discount_final_price\"]";

        private readonly Link a;
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
                var discount = a.FindElement<Contaner>(By.XPath(discountPercent));
                return Math.Abs(Convert.ToInt32(discount.InnerHTML.Replace("%","")));
            }
            catch(Exception)
            {
                return 0;
            }
        }
        private double GetPrice()
        {
            var element = a.FindElement<Contaner>(By.XPath(discountedPrice));
            string s = element.WaitForExists<Contaner>(timeout).InnerHTML;
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
        public SelectedGamePage(IBrowser browser, TimeSpan timeout, AgeVerificationData verificationData, Language ln, string gameName) : base()
        {
            AgeVerificationPage verificationPage = new AgeVerificationPage(browser,timeout);
            if(verificationPage.IsPageOpened)
            {
                verificationPage.Day.SelectByValue(verificationData.Day.ToString());
                verificationPage.Month.SelectByValue(LocalisationKeeper.Get(verificationData.Month, ln));
                verificationPage.Year.SelectByValue(verificationData.Year.ToString());
                verificationPage.Submit();
            }

            Assert.True(BrowserWait.Wait(timeout,(IBrowser b)=>
            {
                return b.Window.FindElement<Contaner>(By.XPath(programNameLocator)).InnerHTML == gameName;
            }, null, typeof(NoSuchElementException)));

            Name = gameName;
            this.timeout = timeout;
            this.language = ln;
        }   

        public string Name { get; }
        private readonly TimeSpan timeout;
        private readonly Language language;

        private readonly string programNameLocator = "//div[@class=\"apphub_AppName\"]";
        private readonly string discountPercent = "//div[@class=\"game_purchase_action\"]//div[1][@class=\"discount_pct\"]";
        private readonly string discountedPrice = "//div[@class=\"game_purchase_action\"]//div[1][@class=\"discount_pct\"]/../div[@class=\"discount_prices\"]/div[@class=\"discount_final_price\"]";
    
        public int Discount 
        {
            get
            {
                var discount = settings.Browser.Window.FindElement<Contaner>(By.XPath(discountPercent)).WaitForExists<Contaner>(timeout);
                return Math.Abs(Convert.ToInt32(discount.InnerHTML.Replace("%","")));
            }
        }

        public double DiscountedPrice
        {
            get
            {
                var price = settings.Browser.Window.FindElement<Contaner>(By.XPath(discountedPrice)).WaitForExists<Contaner>(timeout).InnerHTML;
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
        public AgeVerificationPage(IBrowser browser, TimeSpan timeout) : base()
        {
            this.timeout = timeout;
           // Log(SeleniumWrapper.Logging.LogType.Info,$"Opened page \"{Url}\"","", 0);
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
                element = settings.Browser.Window.FindElement<T>(By.XPath(xPath));
                element.WaitForExists<T>(timeout);
            }

            return element;
        }

        private Select day;
        public Select Day => Getter(dayLocator,ref day);
        private Select month;
        public Select Month => Getter(monthLocator,ref month);
        private Select year;
        public Select Year => Getter(yearLocator,ref year);
        private Link submit;
        private Link OpenPage => Getter(submitLocator,ref submit);

        public void Submit()
        {
            CheckWindow();
            OpenPage.Click();
        }

    }
}