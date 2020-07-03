using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using OpenQA.Selenium;
using SeleniumWrapper;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Elements;
using SeleniumWrapper.Utils;
using SeleniumWrapper.Logging;

using LogType = SeleniumWrapper.Logging.LogType;

namespace Tests.Pages
{
    class GamesPage : BaseForm
    {
        public GamesPage(IBrowser browser, AgeVerificationData verificationData, TimeSpan timeout, Language ln, string gameType, string pathToLogFile) : 
            base(null, true, LoggerCreator.GetLogger(LoggerTypes.FileLogger,null,pathToLogFile))
        {
            this.verificationData = verificationData;
            this.timeout = timeout;
            this.language = ln;
            this.pathToLogFile = pathToLogFile;
            this.gameType = gameType;

            var elem = settings.Browser.Window.FindElement<Text>(By.XPath(headderLocator)).WaitForExists<Text>(timeout);
            Headder = elem.InnerHTML;
        }

        public bool IsPageCorrect
        {
            get
            {
                return Regex.Match(Headder, gameType, RegexOptions.IgnoreCase).Success;
            }
        }

        private readonly AgeVerificationData verificationData;
        private readonly TimeSpan timeout;
        private readonly Language language;
        private readonly string pathToLogFile;
        private readonly string gameType;

        private readonly string itemLocator = "//div[@id=\"TopSellersRows\"]/a";
        private readonly string topSellersLocator = "//div[@id=\"tab_select_TopSellers\"]";
        private readonly string headderLocator = "//h2[@class=\"pageheader\"]";
        private readonly string itemName = ".//div[@class=\"tab_item_name\"]";
        private readonly string discountPercent = ".//div[@class=\"discount_pct\"]";
        private readonly string discountedPrice = ".//div[@class=\"discount_final_price\"]";

        public List<GameItem> Games 
        {
            get
            {
                Log(LogType.Info, "Create list of games", null);
                var div = settings.Browser.Window.FindElement<Contaner>(By.XPath(topSellersLocator)).WaitForExists<Contaner>(timeout);
                div.Click();
                var elements = BrowserWait.Wait(timeout,b=>
                {
                    var elem = settings.Browser.Window.FindElements<Link>(By.XPath(itemLocator));
                    return (elem.Count == 0 ? null : elem);
                },null,typeof(NoSuchElementException));

                List<GameItem> games = new List<GameItem>();
                foreach (var item in elements)
                {
                    string gameName = item.FindElement<Contaner>(By.XPath(itemName)).InnerHTML;
                    SelectedGamePage gamePageCreator()
                    {
                        item.Click();
                        return new SelectedGamePage(timeout,verificationData,language,gameName,pathToLogFile);
                    }
                    int getDiscount()
                    {
                        var discount = item.FindElement<Contaner>(By.XPath(discountPercent));
                        return discount.IsExists ? Math.Abs(Convert.ToInt32(discount.InnerHTML.Replace("%",""))) : 0;
                    }
                    double getPrice()
                    {
                        var element = item.FindElement<Contaner>(By.XPath(discountedPrice));

                        double ans = 0;
                        if(element.IsExists)
                        {
                            Regex regex = new Regex("[^0-9]");
                            string price = regex.Replace(element.InnerHTML,"");
                            ans = (string.IsNullOrEmpty(price) ? 0 : Convert.ToDouble(price));
                        }
                        return ans;
                    }

                    games.Add(new GameItem(gamePageCreator,gameName,getDiscount(),getPrice()));
                }

                return games;
            }
        }
        public string Headder { get; } 

    }

    class GameItem
    {
        public GameItem(Func<SelectedGamePage> gamePageCreator, 
                        string name, int discount, double discountedPrice)
        {
           this.gamePageCreator = gamePageCreator;
           this.Name = name;
           this.Discount = discount;
           this.DiscountedPrice = discountedPrice;
        }

        private readonly Func<SelectedGamePage> gamePageCreator;

        public SelectedGamePage Page => gamePageCreator();
        public string Name { get; }
        public int Discount { get; }
        public double DiscountedPrice { get; }
    }

    class SelectedGamePage : BaseForm
    {
        public SelectedGamePage(TimeSpan timeout, AgeVerificationData verificationData, Language ln, string gameName, string pathToLogFile) : 
            base(null, true, LoggerCreator.GetLogger(LoggerTypes.FileLogger,null,pathToLogFile))
        {
            AgeVerificationPage verificationPage = new AgeVerificationPage(pathToLogFile);
            if(verificationPage.IsPageOpened)
            {
                verificationPage.Day.SelectByValue(verificationData.Day.ToString());
                verificationPage.Month.SelectByValue(LocalisationKeeper.Get(verificationData.Month, ln));
                verificationPage.Year.SelectByValue(verificationData.Year.ToString());
                verificationPage.Submit();
            }
            this.gameName = gameName;

            BrowserWait.Wait(timeout,(IBrowser b)=>
            {
                return b.Window.FindElement<Contaner>(By.XPath(programNameLocator)).InnerHTML == gameName;
            }, null, typeof(NoSuchElementException));

            Name = gameName;
            this.timeout = timeout;
            this.language = ln;
        }   

        public bool IsItCorrectPage => settings.Browser.Window.FindElement<Contaner>(By.XPath(programNameLocator)).InnerHTML == gameName;
        public string Name { get; }
        private readonly TimeSpan timeout;
        private readonly Language language;
        private readonly string gameName;

        private readonly string programNameLocator = "//div[@class=\"apphub_AppName\"]";
        private readonly string discountPercent = "//div[@class=\"game_purchase_action\"]//div[1][@class=\"discount_pct\"]";
        private readonly string discountedPrice = "//div[@class=\"game_purchase_action\"]//div[1][@class=\"discount_pct\"]/../div[@class=\"discount_prices\"]/div[@class=\"discount_final_price\"]";
    
        public int Discount 
        {
            get
            {
                var discount = settings.Browser.Window.FindElement<Contaner>(By.XPath(discountPercent)).WaitForExists<Contaner>(timeout);
                int ans = Math.Abs(Convert.ToInt32(discount.InnerHTML.Replace("%","")));
                Log(LogType.Info, $"Game`s {Name} discount is {ans}",null);
                return ans;
            }
        }

        public double DiscountedPrice
        {
            get
            {
                var price = settings.Browser.Window.FindElement<Contaner>(By.XPath(discountedPrice)).WaitForExists<Contaner>(timeout).InnerHTML;
                Regex regex = new Regex("[^0-9]");
                price = regex.Replace(price,"");
                double ans = (string.IsNullOrEmpty(price) ? 0 :Convert.ToDouble(price));
                Log(LogType.Info, $"Game`s {Name} price is {ans}",null);
                return ans;
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
        public AgeVerificationPage(string pathToLogFile) : 
            base(null, true, LoggerCreator.GetLogger(LoggerTypes.FileLogger,null,pathToLogFile))
        {
        }

        public bool IsPageOpened
        {
            get
            {
                if(!Day.IsExists || !Month.IsExists || !Year.IsExists || !OpenPage.IsExists)
                {
                    return false;
                }
                bool isOpened = Day.Displayed && Month.Displayed && Year.Displayed && OpenPage.Displayed;
                if(isOpened)
                {
                    Log(SeleniumWrapper.Logging.LogType.Info,"Age verification page opened", "DiscountTest");
                }
                return isOpened;
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
            Log(LogType.Info, "Submit age verification",null);
        }

    }
}