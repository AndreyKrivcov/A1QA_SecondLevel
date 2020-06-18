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

namespace Tests.Pages
{
    class MainPage : BaseForm
    {
        public MainPage(IBrowser browser, string url, Language language, 
                        Localisation<MainPageParams,Language> localisation, 
                        TimeSpan timeout, Dictionary<Language, string> languageToLanguageName,
                        string pathToFileName, bool openNewWindow = false, bool threadSaveLogs = false) : 
            base(browser, url, openNewWindow, threadSaveLogs)
        {
            if(!string.IsNullOrEmpty(pathToFileName) && !string.IsNullOrWhiteSpace(pathToFileName))
            {
                loggers.Add(LoggerCreator.GetLogger(LoggerTypes.FileLogger,pathToFileName));
            }
            browser.Window.Maximize();
            browser.Window.Scroll(0,0);

            this.localisation = localisation;
            this.language = language;
            this.timeout = timeout;

            ChangeLanguage(languageToLanguageName);
        }

#region Selectors
        private readonly string LanguageButton = "//span[@id=\"language_pulldown\"]";
        private readonly string InstallSteam = "//a[@class = \"header_installsteam_btn_content\"]";
        private readonly string GamesDiv = "//div[@id=\"genre_tab\"]";
        private readonly string ActionGames = "//div[@class=\"popup_body popup_menu_twocol\"]/div[2]//a[1]";
#endregion

#region Locatisation
        private readonly Language language;
        private readonly Localisation<MainPageParams,Language> localisation;
        
        private void ChangeLanguage(Dictionary<Language, string> languageToLanguageName)
        {
            BaseElement element = browser.Window.FindElement<Span>(By.XPath(LanguageButton));
            Action<string> logger = (string msg)=> Log(LogType.Info,msg,null,0);
            var data = new LanguageDropDown(element,logger,browser,timeout);

            foreach (var item in data.Items)
            {
                if(item.Name == languageToLanguageName[language])
                {
                    item.Click();
                    break;
                }
            }

            System.Threading.Thread.Sleep(10000);            
        }
#endregion

        private readonly TimeSpan timeout;
        public InstallSteam InstallationPage => new InstallSteam(browser,WaitForElement<A>(By.XPath(InstallSteam),timeout));
        public void MouseOverAndClick() 
        {
            var element = WaitForElement<Div>(By.XPath(GamesDiv),timeout);
            browser.MouseUtils.MoveToElement(element).Perform();
            WaitForElement<A>(By.XPath(ActionGames),timeout).Click();
        }
    }
}