using NUnit.Framework;
using SeleniumWrapper.BrowserFabrics;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Utils;
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
           Localisation<Language,Param> l = new Localisation<Language,Param>(); 
           
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