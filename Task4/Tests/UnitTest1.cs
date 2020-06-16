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
            browser.WindowChanged += WindowChanged;
            browser.WindowClosed += WindowClosed;
            browser.BrowserClosed += BrowserClosed;
            browser.BrowserOpened += BrowserOpened;
        }

        [TearDown]
        public void Teardown()
        {
            browser.WindowChanged -= WindowChanged;
            browser.WindowClosed -= WindowClosed;
            browser.BrowserClosed -= BrowserClosed;
            browser.BrowserOpened -= BrowserOpened;
        }

        IBrowser browser;
        LoggersCollection loggers = new LoggersCollection();

        [Test]
        public void Test1()
        {
        }
    }
}