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

            loggers.Add(new [] {LoggerCreator.GetLogger(LoggerTypes.ConsoleLogger, null),
                                LoggerCreator.GetLogger(LoggerTypes.FileLogger,null,config.LogFileName)});

            try
            {
                loginAndPassword = LoginAndPassword.InstanceOrDeserialize(config.PathToLoginAndPasswordFile);

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
        private LoginAndPassword loginAndPassword;
#endregion

        [Test]
        public void Test()
        {
            string method = System.Reflection.MethodBase.GetCurrentMethod().Name;
            loggers.Log(LogType.Info, $"================================ {method} Started ================================", method,null);
            try
            {
                loggers.Log(LogType.Info, "Try to Login", method,1);
                Page page =new Page(TimeSpan.FromSeconds(config.TimeautSeconds), loggers.loggers.ToArray());
                HTMLContent actual = page.SentAuthentificationData(loginAndPassword.Login, loginAndPassword.Password); 

                Assert.True(actual.authenticated);
                Assert.AreEqual(loginAndPassword.Login, actual.user);
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
