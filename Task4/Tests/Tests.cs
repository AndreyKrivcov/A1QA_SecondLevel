using System;
using NUnit.Framework;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Logging;

using Tests.Pages;
using System.Linq;

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
                DownloadUrl = config.DownloadUrl,
                verificationData = new AgeVerificationData
                {
                    Day = config.Day,
                    Month = config.Month,
                    Year = config.Year
                }
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
                Assert.Fail();
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
                var games = homePage.Games(gameType).Games.Where(x=>x.Discount > 0);
                
                int selectedDiscount = (isHigestDiscount ? games.Max(x=>x.Discount) : games.Min(x=>x.Discount));
                var selectedGameItem = games.First(x=>x.Discount == selectedDiscount);

                var gamePage = selectedGameItem.Page;
                Assert.AreEqual(selectedGameItem.Discount,gamePage.Discount);
                Assert.AreEqual(selectedGameItem.DiscountedPrice, gamePage.DiscountedPrice);
            }
            catch(Exception e)
            {
                loggers.Log(e);
                Assert.Fail();
            }

            loggers.Log(LogType.Info,$"================== {System.Reflection.MethodBase.GetCurrentMethod().Name} Finished ==================");
        }

        
    }
}
