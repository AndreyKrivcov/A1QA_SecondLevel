using System;
using OpenQA.Selenium;
using SeleniumWrapper.Logging;
using SeleniumWrapper.Elements;
using Tests.Pages.Shared;
using SeleniumWrapper.Browser;

namespace Tests.Pages
{
    class HomePage : PageBase
    {
        public HomePage(TimeSpan timeout, Logger[] loggers) : base(timeout,loggers)
        {
        }

#region Locators
        private readonly string headerLocator = "//*[@id=\"root\"]/div[2]/section[1]//h1";
#endregion

        protected override string GetHeadder()
        {
            string headder = settings.Browser.Window.FindElement<Text>(By.XPath(headerLocator))
                                   .WaitForDisplayed<Text>(timeout).InnerHTML;

            return headder.Replace("<br>"," ");
        }
    }
}