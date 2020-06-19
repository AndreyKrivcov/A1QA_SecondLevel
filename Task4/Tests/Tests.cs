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
       //     browser.Dispose();
        }

#region Static objects
        IBrowser browser;
        readonly LoggersCollection loggers = new LoggersCollection();
        MainPage homePage;
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
