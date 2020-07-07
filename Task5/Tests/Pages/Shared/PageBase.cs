using System;
using OpenQA.Selenium;
using SeleniumWrapper;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Elements;
using SeleniumWrapper.Logging;

namespace Tests.Pages.Shared
{
    abstract class PageBase : BaseForm
    {
        protected PageBase(TimeSpan timeout, Logger[] loggers, MenuLocators mainMenueLocators = null) : base(null,true, loggers)
        {
            Headder = GetHeadder();
            MainMenue = new Menue(mainMenueLocators?? new MenuLocators(), settings.Browser,timeout,loggers);
        }
        public string Headder { get; }
        public Menue MainMenue { get; }

        protected readonly TimeSpan timeout;

        protected abstract string GetHeadder();

        public class Menue
        {
            public Menue(MenuLocators locators, IBrowser browser, TimeSpan timeout, Logger[] loggers)
            {
                this.locators = locators;
                this.browser = browser;
                this.timeout = timeout;
                this.loggers = loggers;
            }

            private readonly MenuLocators locators;
            private readonly IBrowser browser;
            private readonly TimeSpan timeout;
            private readonly Logger[] loggers;

            public ResearchPage Research
            {
                get
                {
                    ClickAndWait(locators.Research);
                    return new ResearchPage(timeout,loggers);
                }
            }
            public HomePage Home
            {
                get
                {
                    ClickAndWait(locators.Home);
                    return new HomePage(timeout,loggers);
                }
            }

            private void ClickAndWait(string locator)
            {
                browser.Window.FindElement<Link>(By.XPath(locator))
                              .WaitForDisplayed<Link>(timeout).Click();
                browser.Window.WaitForLoading(timeout);
            }
        }

        public class MenuLocators
        {
            public string Home = "//a[@data-linkname=\"header-home\"]";
            public string Research = "(//a[@data-linkname=\"header-research\"])[2]";
        }
    }
}