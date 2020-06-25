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
using SeleniumWrapper.Utils;

namespace Tests.Pages
{
    class DownloadSteam : BaseForm
    {
        public DownloadSteam(IBrowser browser, Link link, string pathToDownload, TimeSpan timeout, string logFilePath, string url) : 
            base(null,true,LoggerCreator.GetLogger(LoggerTypes.FileLogger,null,logFilePath))
        {
            link.Click();
            this.pathToDownload = pathToDownload;
            this.timeout = timeout;

            Log(SeleniumWrapper.Logging.LogType.Info,$"Opened page \"{settings.Browser.Window.Url}\"","", 0);
        }

        private readonly string InstallBtn = "//a[@class=\"about_install_steam_link\"]";
        private readonly string pathToDownload;
        private readonly TimeSpan timeout;
        private readonly string comparationPattern =  "steam_";

        public void Download()
        {
            var files = Directory.GetFiles(pathToDownload);
            ReadOnlyCollection<Link> buttons = settings.Browser.Window.FindElements<Link>(By.XPath(InstallBtn));
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

                ans = BrowserWait.Wait(timeout, (IBrowser) => 
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