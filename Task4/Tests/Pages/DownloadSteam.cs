using System.Linq;

using OpenQA.Selenium;
using SeleniumWrapper;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Elements;
using SeleniumWrapper.Logging;

using System.IO;
using System.Collections.ObjectModel;
using System;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Tests.Pages
{
    class DownloadSteam : BaseForm
    {
        public DownloadSteam(IBrowser browser, A link, string pathToDownload, TimeSpan timeout, string logFilePath, string url) : base(browser)
        {
            link.Click();
            this.Url = browser.Window.Url;
            this.pathToDownload = pathToDownload;
            this.timeout = timeout;
            if(logFilePath != null)
            {
                loggers.Add(LoggerCreator.GetLogger(LoggerTypes.FileLogger,logFilePath));
            }

            Log(SeleniumWrapper.Logging.LogType.Info,$"Opened page \"{Url}\"","", 0);
            Assert.AreEqual(url,Url);
        }

        private readonly string InstallBtn = "//a[@class=\"about_install_steam_link\"]";
        private readonly string pathToDownload;
        private readonly TimeSpan timeout;
        private readonly string comparationPattern =  "steam_";

        public void Download()
        {
            var files = Directory.GetFiles(pathToDownload);
            ReadOnlyCollection<A> buttons = browser.Window.FindElements<A>(By.XPath(InstallBtn));
            buttons.First().Click();

            Assert.True(CheckDownloading(files));
        }

        private bool CheckDownloading(string[] files)
        {
            bool ans;
            try
            {
                bool Compare(string[] files)
                {
                    foreach (var item in files)
                    {
                        if(Regex.Match(item, comparationPattern, RegexOptions.IgnoreCase).Success)
                        {
                            return true;
                        }
                    }
                    return false;
                }

                ans = Wait(timeout, (IBrowser) => 
                {
                    var newFiles = Directory.GetFiles(pathToDownload);
                    return(files.Count() < newFiles.Count() && Compare(newFiles));
                });
            }
            catch(Exception)
            {
                ans = false;
            }

            if(ans)
            {
                Log(SeleniumWrapper.Logging.LogType.Info,"File downloaded",System.Reflection.MethodBase.GetCurrentMethod().Name,2);
            }
            else
            {
                Log(SeleniumWrapper.Logging.LogType.Warning,"File wasn`t downloaded",System.Reflection.MethodBase.GetCurrentMethod().Name,2);
            }

            return ans;
        }
    }
}