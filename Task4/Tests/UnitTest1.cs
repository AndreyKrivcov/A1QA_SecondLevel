using NUnit.Framework;
using SeleniumWrapper.BrowserFabrics;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Elements;
using SeleniumWrapper.Logging;
using OpenQA.Selenium;

using System;
using System.IO;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            LoggersCollection loggers = new LoggersCollection();
            loggers.Add(new [] {LoggerCreator.GetLogger(LoggerTypes.FileLogger,"MyDoubleLogedFile.csv"), 
                                LoggerCreator.GetLogger(LoggerTypes.FileLogger,"MyDoubleLogedFile.csv"),
                                LoggerCreator.GetLogger(LoggerTypes.ConsoleLogger,""),
                                LoggerCreator.GetLogger(LoggerTypes.ConsoleLogger,"")});

            loggers.TestName = "Test # 1";
            loggers.TestStep = 1;
            loggers.Log(SeleniumWrapper.Logging.LogType.Info, "Info log");
            loggers.Log(SeleniumWrapper.Logging.LogType.Warning, "Warning log");
            loggers.Log(SeleniumWrapper.Logging.LogType.Error, "Info Error");
            loggers.Log(SeleniumWrapper.Logging.LogType.Fatal, "Info fatal");

            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

        }
    }
}