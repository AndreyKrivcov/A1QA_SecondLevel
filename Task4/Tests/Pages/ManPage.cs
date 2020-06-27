using System;
using System.Linq;

using OpenQA.Selenium;
using SeleniumWrapper;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Elements;
using SeleniumWrapper.Logging;
using SeleniumWrapper.Utils;

using LogType = SeleniumWrapper.Logging.LogType;

namespace Tests.Pages
{
    struct MainPageSettings
    {
        public Language Language;
        public TimeSpan Timeout;
        public string PathToLogFile; 
        public string PathToDownload;
        public string DownloadUrl;

        public AgeVerificationData verificationData;
    }

    class MainPage : BaseForm
    {
        public MainPage(MainPageSettings settings) : base(null, true, LoggerCreator.GetLogger(LoggerTypes.FileLogger,null,settings.PathToLogFile))
        {
            pageSettings = settings;

            if(!string.IsNullOrEmpty(settings.PathToLogFile) && !string.IsNullOrWhiteSpace(settings.PathToLogFile))
            {
                loggers.Add();
            }

            ChangeLanguage();
        }

#region Selectors
        private readonly string LanguageButton = "//span[@id=\"language_pulldown\"]";
        private readonly string InstallSteam = "//a[@class = \"header_installsteam_btn_content\"]";
        private readonly string GamesDiv = "//div[@id=\"genre_tab\"]";
        private readonly string GamesA = "//div[@class=\"popup_body popup_menu_twocol\"]//a[contains(text(),\"{0}\")]";
#endregion

        private readonly MainPageSettings pageSettings;

#region Locatisation
        
        private void ChangeLanguage()
        {
            BaseElement element = settings.Browser.Window.FindElement<Contaner>(By.XPath(LanguageButton));
            Action<string> logger = (string msg)=> Log(LogType.Info,msg,null,null);
            var data = new LanguageDropDown(element,logger,settings.Browser,pageSettings.Timeout);

            if(data.Items.Any(x=>x.Name == LocalisationKeeper.LanguageNames[GenericParams.Language][pageSettings.Language]))
            {
                foreach (var item in data.Items)
                {
                    if(item.Name == LocalisationKeeper.LanguageNames[GenericParams.Language][pageSettings.Language])
                    {
                        item.Click();
                        BrowserWait.Wait(pageSettings.Timeout,(IBrowser b) =>
                        {
                            return b.Window.Title == LocalisationKeeper.Get(Test_1.Title,pageSettings.Language);
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
                Log(LogType.Info, "Opening Steam downloading page",null,null);
                var link = settings.Browser.Window.FindElement<Link>(By.XPath(InstallSteam))
                    .WaitForDisplayed<Link>(pageSettings.Timeout);
                return new DownloadSteam(settings.Browser,
                    link,
                    pageSettings.PathToDownload, pageSettings.Timeout, pageSettings.PathToLogFile,
                    pageSettings.DownloadUrl);
            }
        }

        private Link GetDropDownElement(string divLocator, string elementLocator)
        {
            Log(LogType.Info,$"Mouse over to dropdown element",null,null);
            return BrowserWait.Wait(pageSettings.Timeout,(IBrowser b)=>
            {
                var div = b.Window.FindElement<Contaner>(By.XPath(divLocator));
                b.MouseActions.MoveToElement(div).Perform();
            
                Link element  = b.Window.FindElement<Link>(By.XPath(elementLocator));
                return (element.IsExists && element.Displayed ? element : null);
            }, null, typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        }

        public GamesPage Games(Test_2 gameType)
        {
            Log(LogType.Info,$"Open pagewith list of games",null,null);
            var element = GetDropDownElement(GamesDiv,string.Format(GamesA,LocalisationKeeper.Get(gameType,pageSettings.Language)));
            element.Click();
                
            return new GamesPage(settings.Browser, pageSettings.verificationData, 
                    pageSettings.Timeout,pageSettings.Language, 
                    LocalisationKeeper.Get(gameType,pageSettings.Language),
                    pageSettings.PathToLogFile);
        }
    }
}