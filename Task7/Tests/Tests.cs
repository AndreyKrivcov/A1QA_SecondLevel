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
        public void Test_Cars()
        {
            string method = System.Reflection.MethodBase.GetCurrentMethod().Name;
            loggers.Log(LogType.Info, $"================================ {method} Started ================================", method,null);
            try
            {
               var page = new ExamplePage();
               page.Cookies.Max(x=> x.Name);
            }
            catch(Exception e)
            {
                loggers.Log(e,method,0);
            }

            loggers.Log(LogType.Info, $"================================ {method} Finished ================================",method,null); 
        }
    }
}
