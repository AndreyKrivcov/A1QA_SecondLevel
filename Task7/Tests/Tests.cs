using System;
using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Logging;
using SeleniumWrapper.BrowserFabrics;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            config = Config.InstanceOrDeserialize(fileWithSettings);
            cookieData = CookieData.InstanceOrDeserialize(config.CookieDataFile);

            loggers.Add(new [] {LoggerCreator.GetLogger(LoggerTypes.ConsoleLogger, null),
                                LoggerCreator.GetLogger(LoggerTypes.FileLogger,null,config.LogFileName)});

            try
            {
                browser = BrowserFabric.GetBrowser(config.Browser);
                browser.Window.Maximize();
                browser.Window.Url = config.MainUrl;
            }
            catch(Exception e)
            {
                loggers.Log(e,"TestBefore",null);
            }
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
        private CookieData cookieData;
#endregion

        [Test]
        public void Test_Cookeis()
        {
            string method = System.Reflection.MethodBase.GetCurrentMethod().Name;
            loggers.Log(LogType.Info, $"================================ {method} Started ================================", method,null);
            try
            {
                loggers.Log(LogType.Info, "Add cookie", method, 1);
                var page = new ExamplePage();
                page.Cookies.Add(new OpenQA.Selenium.Cookie(cookieData.First.Key, cookieData.First.Value));
                page.Cookies.Add(new OpenQA.Selenium.Cookie(cookieData.Second.Key, cookieData.Second.Value));
                page.Cookies.Add(new OpenQA.Selenium.Cookie(cookieData.Third.Key, cookieData.Third.Value));

                var cookies = page.Cookies.AsReadonly();
                Assert.True(cookies.Any(x=>x.Name == cookieData.First.Key && x.Value == cookieData.First.Value));
                Assert.True(cookies.Any(x=>x.Name == cookieData.Second.Key && x.Value == cookieData.Second.Value));
                Assert.True(cookies.Any(x=>x.Name == cookieData.Third.Key && x.Value == cookieData.Third.Value));

                loggers.Log(LogType.Info, "Delete first cookie", method, 2);
                page.Cookies.Delete(cookieData.First.Key);
                cookies = page.Cookies.AsReadonly();
                Assert.False(cookies.Any(x=>x.Name == cookieData.First.Key));

                loggers.Log(LogType.Info, "Update third cookie", method, 3);
                page.Cookies[cookieData.Third.Key] = new OpenQA.Selenium.Cookie(cookieData.Third.Key, cookieData.NewCookie3Value);
                Assert.AreEqual(cookieData.NewCookie3Value, page.Cookies[cookieData.Third.Key].Value);

                loggers.Log(LogType.Info, "Clear cookie", method, 4);
                page.Cookies.Clear();
                Assert.AreEqual(page.Cookies.AsReadonly().Count, 0);

            }
            catch(Exception e)
            {
                loggers.Log(e,method,0);
            }

            loggers.Log(LogType.Info, $"================================ {method} Finished ================================",method,null); 
        }
    }
}
