using System;
using System.IO;
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
            Config config;
            if(File.Exists(fileWithSettings))
            {
                config = Config.Deserialization();
            }
            else
            {
                config = new Config();
                config.Serialization();
            }

            loggers.Add(new [] {LoggerCreator.GetLogger(LoggerTypes.ConsoleLogger, null),
                                LoggerCreator.GetLogger(LoggerTypes.FileLogger,null,config.LogFileName)});

            browser = BrowserCreator.GetConfiguredBrowser(config.browser);
            browser.Window.Url = config.MainUrl;
            browser.Window.Maximize();
            browser.Window.Scroll(0,0);

            homePage = new MainPage(new MainPageSettings
            {
                Language = config.Language,
                PathToDownload = config.PathToDownload,
                PathToLogFile = config.LogFileName,
                Timeout = TimeSpan.FromSeconds(config.TimeautSeconds),
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

        IBrowser browser;
        readonly LoggersCollection loggers = new LoggersCollection();
        readonly string fileWithSettings = "TestConfigurationFile.txt";
        MainPage homePage;


        [Test]
        public void DownloadSteam()
        {
            loggers.Log(LogType.Info,$"================== {System.Reflection.MethodBase.GetCurrentMethod().Name} Start ==================",
                System.Reflection.MethodBase.GetCurrentMethod().Name,null);
            try
            {
                homePage.InstallationPage.Download();
            }
            catch(Exception e)
            {
                loggers.Log(e,System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Assert.Fail();
            }
            loggers.Log(LogType.Info,$"================== {System.Reflection.MethodBase.GetCurrentMethod().Name} Finished ==================",
                System.Reflection.MethodBase.GetCurrentMethod().Name, null);
        }
       
        [TestCase(Test_2.Action, true)]
        [TestCase(Test_2.Indie, false)]
        public void DiscountTest(Test_2 gameType, bool isHigestDiscount)  
        {
            loggers.Log(LogType.Info,$"================== {System.Reflection.MethodBase.GetCurrentMethod().Name} Start ==================",
                System.Reflection.MethodBase.GetCurrentMethod().Name, null);
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
                loggers.Log(e,System.Reflection.MethodBase.GetCurrentMethod().Name,null);
                Assert.Fail();
            }

            loggers.Log(LogType.Info,$"================== {System.Reflection.MethodBase.GetCurrentMethod().Name} Finished ==================",
                System.Reflection.MethodBase.GetCurrentMethod().Name, null);
        }

        
    }
}
