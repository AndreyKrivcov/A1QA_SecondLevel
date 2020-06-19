using System;
using System.Linq;
using System.Collections.Generic;

using OpenQA.Selenium;
using SeleniumWrapper;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Elements;
using SeleniumWrapper.Logging;
using SeleniumWrapper.Utils;

using LogType = SeleniumWrapper.Logging.LogType;
using System.Collections.ObjectModel;
using NUnit.Framework;

namespace Tests.Pages
{
    struct MainPageSettings
    {
        public IBrowser Browser;
        public string Url; 
        public string DownloadUrl;
        public Language Language;
        public TimeSpan Timeout;
        public string PathToLogFile; 
        public string PathToDownload;

        public AgeVerificationData verificationData;
    }

    class MainPage : BaseForm
    {
        public MainPage(MainPageSettings settings) : 
            base(settings.Browser, settings.Url, false,false)
        {
            this.settings = settings;

            if(!string.IsNullOrEmpty(settings.PathToLogFile) && !string.IsNullOrWhiteSpace(settings.PathToLogFile))
            {
                loggers.Add(LoggerCreator.GetLogger(LoggerTypes.FileLogger,settings.PathToLogFile));
            }
            browser.Window.Maximize();
            browser.Window.Scroll(0,0);

            Log(SeleniumWrapper.Logging.LogType.Info,$"Opened page \"{Url}\"","", 0);

            ChangeLanguage();

            Assert.AreEqual(settings.Url,browser.Window.Url);
        }

#region Selectors
        private readonly string LanguageButton = "//span[@id=\"language_pulldown\"]";
        private readonly string InstallSteam = "//a[@class = \"header_installsteam_btn_content\"]";
        private readonly string GamesDiv = "//div[@id=\"genre_tab\"]";
        private readonly string GamesA = "//div[@class=\"popup_body popup_menu_twocol\"]//a[contains(text(),\"{0}\")]";
#endregion

        private readonly MainPageSettings settings;

#region Locatisation
        
        private void ChangeLanguage()
        {
            BaseElement element = browser.Window.FindElement<Span>(By.XPath(LanguageButton));
            Action<string> logger = (string msg)=> Log(LogType.Info,msg,null,1);
            var data = new LanguageDropDown(element,logger,browser,settings.Timeout);

            if(data.Items.Any(x=>x.Name == LocalisationKeeper.LanguageNames[settings.Language]))
            {
                foreach (var item in data.Items)
                {
                    if(item.Name == LocalisationKeeper.LanguageNames[settings.Language])
                    {
                        item.Click();
                        Wait(settings.Timeout,(IBrowser b) =>
                        {
                            return b.Window.Title == LocalisationKeeper.Get(Test_1.Title,settings.Language);
                        });
                        break;
                    }
                }
            }
            else
            {
                element.Click();
            }           
        }
#endregion

        public DownloadSteam InstallationPage
        {
            get
            {
                return new DownloadSteam(browser,
                    WaitForElement<A>(By.XPath(InstallSteam),settings.Timeout),
                    settings.PathToDownload, settings.Timeout, settings.PathToLogFile,
                    settings.DownloadUrl);
            }
        }

        private A GetDropDownElement(string divLocator, string elementLocator)
        {
            return Wait(settings.Timeout,(IBrowser b)=>
            {
                var div = b.Window.FindElement<Div>(By.XPath(divLocator));
                b.MouseUtils.MoveToElement(div).Perform();
            
                A element  = b.Window.FindElement<A>(By.XPath(elementLocator));
                return (element.IsExists && element.Displayed ? element : null);
            }, null, typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        }

        public GamesPage Games(Test_2 gameType)
        {
            var element = GetDropDownElement(GamesDiv,string.Format(GamesA,LocalisationKeeper.Get(gameType,settings.Language)));
            element.Click();
                
            return new GamesPage(browser, settings.verificationData, 
                    settings.Timeout,settings.Language, 
                    LocalisationKeeper.Get(gameType,settings.Language));
        }
    }
}