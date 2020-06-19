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

            homePage = new MainPage(new MainPageSettings
            {
                Browser = browser,
                Language = config.Language,
                PathToDownload = config.PathToDownload,
                PathToLogFile = config.LogFileName,
                Timeout = TimeSpan.FromSeconds(config.TimeautSeconds),
                Url = config.MainUrl,
                DownloadUrl = config.DownloadUrl
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
#endregion


        [Test]
        public void DownloadSteam()
        {
            loggers.Log(LogType.Info,$"================== {System.Reflection.MethodBase.GetCurrentMethod().Name} Start ==================");
            try
            {
                homePage.InstallationPage.Download();
            }
            catch(Exception e)
            {
                loggers.Log(e);
            }
            loggers.Log(LogType.Info,$"================== {System.Reflection.MethodBase.GetCurrentMethod().Name} Finished ==================");
        }
       
        [TestCase(Test_2.Action, true)]
        [TestCase(Test_2.Indie, false)]
        public void DiscountTest(Test_2 gameType, bool isHigestDiscount)  
        {
            loggers.Log(LogType.Info,$"================== {System.Reflection.MethodBase.GetCurrentMethod().Name} Start ==================");
            try
            {
                browser.Window.Url = "https://store.steampowered.com/agecheck/app/1282690/";
                var verificationPage = new AgeVerificationPage(browser,TimeSpan.FromMinutes(1));
                if(verificationPage.IsPageOpened)
                {
                    var month = verificationPage.Month.Options;
                    foreach (var item in month)
                    {
                        loggers.Log(LogType.Warning, item.InnerHTML);
                    }
                }
            }
            catch(Exception e)
            {
                loggers.Log(e);
            }

            loggers.Log(LogType.Info,$"================== {System.Reflection.MethodBase.GetCurrentMethod().Name} Finished ==================");
        }

        
    }
}
