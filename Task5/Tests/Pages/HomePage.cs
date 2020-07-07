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

        private readonly string headerLocator = "//*[@id=\"root\"]/div[2]/section[1]//h1";

        protected override string GetHeadder()
        {
            string headder = settings.Browser.Window.FindElement<GenericElement>(By.XPath(headerLocator))
                                   .WaitForDisplayed<GenericElement>(timeout).InnerHTML;

            return headder.Replace("<br>"," ");
        }
    }

    class GenericElement : BaseElement
    {
        public GenericElement(WebElementKeeper element) : base(element)
        {
        }
    }
}