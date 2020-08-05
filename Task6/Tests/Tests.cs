using System;
using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Logging;
using SeleniumWrapper.BrowserFabrics;
using SeleniumWrapper.Utils;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            config = Config.InstanceOrDeserialize(fileWithSettings);
            expectedResults = ExpectedResults.InstanceOrDeserialize(config.ExpectedAnswersAndTitles);

            loggers.Add(new [] {LoggerCreator.GetLogger(LoggerTypes.ConsoleLogger, null),
                                LoggerCreator.GetLogger(LoggerTypes.FileLogger,null,config.LogFileName)});

            try
            {
                browser = BrowserFabric.GetBrowser(config.Browser);
                browser.Window.Maximize();
                browser.Window.Url = config.Url;
            }
            catch(Exception e)
            {
                loggers.Log(e,"Test before",null);
                Assert.Fail();
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
        private ExpectedResults expectedResults;
#endregion

        [Test]
        public void Test_ModalWindows()
        {
            string method = System.Reflection.MethodBase.GetCurrentMethod().Name;
            loggers.StartTest(method);
            try
            {
                AlertsPage page = new AlertsPage(loggers.loggers.ToArray());
                
                loggers.Log(LogType.Info,"Alert",method,1);
                SimpleAlert simpleAlert = page.SimpleAlert();
                Assert.AreEqual(expectedResults.AlertTitle, simpleAlert.Text);
                simpleAlert.Confirm();
                Assert.AreEqual(expectedResults.AlertAnswer, page.Answer);

                loggers.Log(LogType.Info, "Confirm", method, 2);
                ConfirmAlert confirmAlert = page.ConfirmAlert();
                Assert.AreEqual(expectedResults.ConfirmTitle,confirmAlert.Text);
                confirmAlert.Confirm();
                Assert.AreEqual(expectedResults.ConfirmPositiveAnswer, page.Answer);

                loggers.Log(LogType.Info, "Prompt", method, 3);
                PromtAlert promtAlert = page.PromtAlert();
                Assert.AreEqual(expectedResults.PromptTitle, promtAlert.Text);
                string msg = StringUtils.RandomString();
                promtAlert.Message = msg;
                promtAlert.Confirm();
                Assert.AreEqual(expectedResults.PromptAnswer(msg), page.Answer);
            }
            catch(Exception e)
            {
                loggers.Log(e,method,0);
                Assert.Fail();
            }

            loggers.EndTest(method); 
        }
    }
}
