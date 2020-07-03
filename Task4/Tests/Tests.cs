using System;
using System.IO;
using NUnit.Framework;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Logging;

using Tests.Pages;
using System.Linq;
using System.Text.RegularExpressions;
using SeleniumWrapper.Utils;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            if(File.Exists(fileWithSettings))
            {
                config = Config.Deserialization();
            }
            else
            {
                config = new Config();
                config.Serialization();
            }

            LocalisationKeeper.Configure(config.PathToLocalisationForTest_1, config.PathToLocalisationForTest_2,
                                        config.PathToMonthLocalisation,config.PathToLanguageNames);

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

#region Settings
        IBrowser browser;
        readonly LoggersCollection loggers = new LoggersCollection();
        readonly string fileWithSettings = "TestConfigurationFile.txt";
        private Config config;
        MainPage homePage;
#endregion

        [Test]
        public void DownloadSteam()
        {
            loggers.Log(LogType.Info,$"================== {System.Reflection.MethodBase.GetCurrentMethod().Name} Start ==================",
                System.Reflection.MethodBase.GetCurrentMethod().Name,null);
            try
            {
#region Step 1
                var instalationPage = homePage.InstallationPage;
                bool check = instalationPage.IsItDownloadingPage;
                loggers.Log(LogType.Info,$"Is it instalation page ? {check}", System.Reflection.MethodBase.GetCurrentMethod().Name,1);
                Assert.True(check);
#endregion
                
#region Step 2
                var files = Directory.GetFiles(config.PathToDownload);
                instalationPage.Download();
                Assert.True(CheckDownloading(files,instalationPage.ComparationPattern,TimeSpan.FromSeconds(config.TimeautSeconds),config.PathToDownload,2));
#endregion
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
#region Step 1
                var gamesPage = homePage.Games(gameType);
                bool isPageCorrect = gamesPage.IsPageCorrect;
                loggers.Log(LogType.Info,$"Isit games list page ? = {isPageCorrect}", System.Reflection.MethodBase.GetCurrentMethod().Name, 1);
                Assert.True(gamesPage.IsPageCorrect);
#endregion

#region Step 2
                var games = gamesPage.Games.Where(x=>x.Discount > 0);
                games.ToList().ForEach(x=>
                {
                    loggers.Log(LogType.Info,$"Game {x.Name} with discount = {x.Discount} and price = {x.DiscountedPrice}",
                                System.Reflection.MethodBase.GetCurrentMethod().Name, 2);
                });
#endregion

#region Step 3      
                int selectedDiscount = (isHigestDiscount ? games.Max(x=>x.Discount) : games.Min(x=>x.Discount));
                var selectedGameItem = games.First(x=>x.Discount == selectedDiscount);

                var gamePage = selectedGameItem.Page;

                Assert.AreEqual(selectedGameItem.Discount,gamePage.Discount);
                Assert.AreEqual(selectedGameItem.DiscountedPrice, gamePage.DiscountedPrice);

                loggers.Log(LogType.Info, $"Select game \"{selectedGameItem.Name}\" discount = {selectedGameItem.Discount} and price = {selectedGameItem.DiscountedPrice}", 
                            System.Reflection.MethodBase.GetCurrentMethod().Name, 3);
#endregion
            }
            catch(Exception e)
            {
                loggers.Log(e,System.Reflection.MethodBase.GetCurrentMethod().Name,null);
                Assert.Fail();
            }

            loggers.Log(LogType.Info,$"================== {System.Reflection.MethodBase.GetCurrentMethod().Name} Finished ==================",
                System.Reflection.MethodBase.GetCurrentMethod().Name, null);
        }

        private bool CheckDownloading(string[] files, string comparationPattern, TimeSpan timeout, string pathToDownload, int step)
        {
            bool ans;
            try
            {
                bool Compare(string[] files)
                {
                    foreach (var item in files)
                    {
                        if(Regex.Match(item, comparationPattern, RegexOptions.IgnoreCase).Success)
                        {
                            return true;
                        }
                    }
                    return false;
                }

                ans = BrowserWait.Wait(timeout, browser => 
                {
                    var newFiles = Directory.GetFiles(pathToDownload);
                    return(files.Count() < newFiles.Count() && Compare(newFiles));
                });
            }
            catch(Exception)
            {
                ans = false;
            }

            if(ans)
            {
                loggers.Log(LogType.Info,"File downloaded",System.Reflection.MethodBase.GetCurrentMethod().Name, step);
            }
            else
            {
                loggers.Log(LogType.Warning,"File wasn`t downloaded",System.Reflection.MethodBase.GetCurrentMethod().Name, step);
            }

            return ans;
        }
    }
}
