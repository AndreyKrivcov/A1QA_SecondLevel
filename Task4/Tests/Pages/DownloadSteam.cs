using System.Linq;
using System.Collections.ObjectModel;
using System;

using OpenQA.Selenium;
using SeleniumWrapper;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Elements;
using SeleniumWrapper.Logging;

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
        }

        private readonly string InstallBtn = "//a[@class=\"about_install_steam_link\"]";
        private readonly string pathToDownload;
        private readonly TimeSpan timeout;
        public readonly string ComparationPattern =  "steam_";

        public void Download()
        {
            Log(SeleniumWrapper.Logging.LogType.Info,"Steam downloading began",null);
            
            ReadOnlyCollection<Link> buttons = settings.Browser.Window.FindElements<Link>(By.XPath(InstallBtn));
            buttons.First().Click();
        }

        public bool IsItDownloadingPage
        {
            get
            {
                ReadOnlyCollection<Link> buttons = settings.Browser.Window.FindElements<Link>(By.XPath(InstallBtn));
                return buttons.First().IsExists;
            }
        }
    }
}