using System;
using NUnit.Framework;
using SeleniumWrapper.Browser;
using SeleniumWrapper.BrowserFabrics;
using SeleniumWrapper.Logging;
using SeleniumWrapper.Utils;

using System.Collections.Generic;

using Tests.Pages;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            Config config = new Config();

            loggers.Add(new [] {LoggerCreator.GetLogger(LoggerTypes.ConsoleLogger,""),
                                LoggerCreator.GetLogger(LoggerTypes.FileLogger,config.LogFileName)});

            browser = BrowserCreator.GetConfiguredBrowser(config.browser);

            Localisation<MainPageParams,Language> mainPageLocalisation 
                = new Localisation<MainPageParams,Language>();
            

            homePage = new MainPage(new MainPageSettings
            {
                Browser = browser,
                Language = config.Language,
                LanguageToLanguageName = languageNames,
                Localisation = mainPageLocalisation,
                PathToDownload = config.PathToDownload,
                PathToLogFile = config.LogFileName,
                Timeout = TimeSpan.FromSeconds(config.TimeautSeconds),
                Url = config.MainUrl
            });
        }

        [TearDown]
        public void Teardown()
        {
            browser.Dispose();
        }

#region Static objects
        IBrowser browser;
        readonly LoggersCollection loggers = new LoggersCollection();
        MainPage homePage;
        readonly Dictionary<Language,string> languageNames = new Dictionary<Language, string>
            {
                {Language.Ru, "Русский (Russian)"},
                {Language.En, "English (английский)"}
            };
#endregion


        [Test]
        public void DownloadSteam()
        {
            try
            {
                homePage.InstallationPage.Download();
            }
            catch(Exception e)
            {
                loggers.Log(e);
            }
        }
    }
}
