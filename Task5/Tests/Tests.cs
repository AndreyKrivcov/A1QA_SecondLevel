using System.IO;
using NUnit.Framework;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Logging;

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

            loggers.Add(new [] {LoggerCreator.GetLogger(LoggerTypes.ConsoleLogger, null),
                                LoggerCreator.GetLogger(LoggerTypes.FileLogger,null,config.LogFileName)});

            browser = BrowserCreator.GetConfiguredBrowser(config.browser);
           
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

    }
}
