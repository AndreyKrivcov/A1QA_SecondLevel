using NUnit.Framework;
using SeleniumWrapper.BrowserFabrics;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Elements;
using SeleniumWrapper.Logging;
using OpenQA.Selenium;

using System;
using System.IO;

using LogType = SeleniumWrapper.Logging.LogType;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            loggers.Add(new [] {LoggerCreator.GetLogger(LoggerTypes.ConsoleLogger,"")});
            browser = new BrowserFabric().GetBrowser(BrowserType.Chrome);
        }

        [TearDown]
        public void Teardown()
        {
            browser.Dispose();
        }

        IBrowser browser;
        LoggersCollection loggers = new LoggersCollection();

        [Test]
        public void Test1()
        {
            browser.Window.Url = "https://market.yandex.ru/catalog--detskie-tovary/54421";
            var elem = browser.Window.FindElement(By.XPath("/html/body/div[2]/div[3]/div[6]/div/div/div/div[2]/div[7]/div/div/div/div/div/div[2]/div/div[1]/div/div/div[3]"));
            browser.Window.Scroll(0,1000);
            
            

            elem.WaitForExists(TimeSpan.FromMinutes(1));
        }
    }
}