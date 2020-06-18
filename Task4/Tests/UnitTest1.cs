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
            loggers.Add(new [] {LoggerCreator.GetLogger(LoggerTypes.ConsoleLogger,"")});
            browser = BrowserFabric.GetBrowser(BrowserType.Chrome);
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
        
            Dictionary<Language,string> ls = new Dictionary<Language, string>
            {
                {Language.Ru, "Русский (Russian)"},
                {Language.En, "English (английский)"}
            };

            Localisation<MainPageParams,Language> localisation = new Localisation<MainPageParams,Language>();
            localisation.AddOrReplace(MainPageParams.Games,Language.Ru,"Игры");
            localisation.AddOrReplace(MainPageParams.Games,Language.En,"Games");


            MainPage mainPage = new MainPage(browser,"https://store.steampowered.com/",Language.En,localisation,TimeSpan.FromMinutes(1),ls,null);
            mainPage.MouseOver();
        }
    }
}
