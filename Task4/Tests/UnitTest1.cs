using NUnit.Framework;
using SeleniumWrapper.BrowserFabrics;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Utils;
using SeleniumWrapper.Logging;
using OpenQA.Selenium;

using System;
using System.IO;

using LogType = SeleniumWrapper.Logging.LogType;
using SeleniumWrapper.Elements;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            loggers.Add(new [] {LoggerCreator.GetLogger(LoggerTypes.ConsoleLogger,"")});
           // browser = BrowserFabric.GetBrowser(BrowserType.Chrome);
        }

        [TearDown]
        public void Teardown()
        {
            //browser.Dispose();
        }

        IBrowser browser;
        LoggersCollection loggers = new LoggersCollection();

        [Test]
        public void Test1()
        {
            using(IBrowser b = BrowserFabric.GetBrowser(BrowserType.FireFox))
            {
                try
                {
                    b.Window.Maximize();
                    b.Window.Url = "https://store.steampowered.com/";
                    var a = b.Window.FindElement<A>(By.XPath("//*[@id=\"genre_tab\"]/span/a[1]"));
                    string s = a.Coords;
                    loggers.Log(LogType.Warning,s);
                }
                catch(Exception e)
                {
                    loggers.Log(e);
                }
            }
           
        }
    }

    enum Language
    {
        En,
        Rus
    }

    enum Param
    {
        Param_1,
        Param_2
    }
}