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
            using(IBrowser b = BrowserFabric.GetBrowser(BrowserType.Chrome))
            {
                try
                {
                    b.Window.Maximize();
                    b.Window.Url = "https://store.steampowered.com/agecheck/app/1282690/";
                  //  b.Window.Scroll(0,5000);

                    var s = b.Window.FindElement<Select>(By.XPath("//select[@id=\"ageYear\"]"));
                    
                    loggers.Log(LogType.Warning, s.AccessKey);
                    loggers.Log(LogType.Warning, s.Form);
                    loggers.Log(LogType.Warning, s.Name);
                    loggers.Log(LogType.Warning, s.SelectedOption.InnerHTML);
                    s.SelectByIndex(10);
                    loggers.Log(LogType.Warning, s.SelectedOption.InnerHTML);
                    loggers.Log(LogType.Warning, s.SelectedOption.InnerHTML);

                    foreach (var item in s.Options)
                    {
                        loggers.Log(LogType.Warning, item.InnerHTML);
                    }

                    loggers.Log(LogType.Info,"");
                    loggers.Log(LogType.Warning,"");
                    loggers.Log(LogType.Error,"");
                    loggers.Log(LogType.Fatal,"");
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