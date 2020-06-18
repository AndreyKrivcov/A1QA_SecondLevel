using System;
using NUnit.Framework;
using SeleniumWrapper.Browser;
using SeleniumWrapper.BrowserFabrics;
using SeleniumWrapper.Logging;
using SeleniumWrapper.Utils;

using System.Collections.Generic;

using Tests.Pages;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            loggers.Add(new [] {LoggerCreator.GetLogger(LoggerTypes.ConsoleLogger,"")});

            ChromeOptions options = new ChromeOptions();
            options.AddUserProfilePreference("safebrowsing.enabled","false");

            FirefoxOptions ffOptions = new FirefoxOptions();
            ffOptions.AddAdditionalOption("safebrowsing.enabled","false");
            

            browser = BrowserFabric.GetBrowser(BrowserType.FireFox,ffOptions);
        }

        [TearDown]
        public void Teardown()
        {
         //   browser.Dispose();
        }

        IBrowser browser;
        LoggersCollection loggers = new LoggersCollection();

        [Test]
        public void Test1()
        {
        try
        {
            Dictionary<Language,string> ls = new Dictionary<Language, string>
            {
                {Language.Ru, "Русский (Russian)"},
                {Language.En, "English (английский)"}
            };

            Localisation<MainPageParams,Language> localisation = new Localisation<MainPageParams,Language>();
            localisation.AddOrReplace(MainPageParams.Games,Language.Ru,"Игры");
            localisation.AddOrReplace(MainPageParams.Games,Language.En,"Games");


            MainPage mainPage = new MainPage(browser,"https://store.steampowered.com/",Language.Ru,localisation,TimeSpan.FromMinutes(1),ls,null);
           // mainPage.MouseOverAndClick();
            mainPage.InstallationPage.Install();
        }
        catch(Exception e)
        {
            loggers.Log(e);
        }
        }
    }
}
