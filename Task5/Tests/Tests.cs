using System;

using NUnit.Framework;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Logging;
using SeleniumWrapper.BrowserFabrics;

using LogType = SeleniumWrapper.Logging.LogType;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            config = Config.InstanceOrDeserialize(fileWithSettings);

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
                loggers.Log(e,"Test before", null);
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
#endregion

        [Test]
        public void Test()
        {
            string method = System.Reflection.MethodBase.GetCurrentMethod().Name;
            loggers.Log(LogType.Info, $"================================ {method} Started ================================", method,null);
            try
            {

            }
            catch(Exception e)
            {
                loggers.Log(e,method,null);
                Assert.Fail();
            }
            loggers.Log(LogType.Info, $"================================ {method} Finished ================================",method,null); 
        }
    }
}
