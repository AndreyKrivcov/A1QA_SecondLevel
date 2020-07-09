using System;
using OpenQA.Selenium;
using SeleniumWrapper;
using SeleniumWrapper.Elements;
using SeleniumWrapper.Logging;
using Tests.Pages.Shared;

namespace Tests.Pages
{
    class SideBySidePage : PageBase
    {
        public SideBySidePage(TimeSpan timeout, Logger[] loggers) : base(timeout,loggers)
        {
            CarsSelector = new CarsSelector(
                settings.Browser.Window.FindElement<Select>(By.XPath(makeLocator)).WaitForDisplayed<Select>(timeout),
                settings.Browser.Window.FindElement<Select>(By.XPath(modelLocator)).WaitForDisplayed<Select>(timeout),
                settings.Browser.Window.FindElement<Select>(By.XPath(yearLocator)).WaitForDisplayed<Select>(timeout),
                selectorsForbiddenValues
            );
        }

#region Locators
        private readonly string headdersLocator = "//h1[@class=\"cui-alpha compare-head-h1-semi\"]";
        private readonly string makeLocator = "//select[@id=\"make-dropdown\"]";
        private readonly string modelLocator = "//select[@id=\"model-dropdown\"]";
        private readonly string yearLocator = "//select[@id=\"year-dropdown\"]";
        private readonly string doneLocator = "//button[@class=\"done-button\"]";
#endregion

        private readonly ForbiddenValues selectorsForbiddenValues = new ForbiddenValues
        {
            Make = "",
            Model = "",
            Year = ""
        };

        public CarsSelector CarsSelector { get; }
        public ComparationPage StartComparing()
        {
            string make = CarsSelector.Make.Selected;
            string model = CarsSelector.Model.Selected;
            int year = Convert.ToInt32(CarsSelector.Year.Selected);

            settings.Browser.Window.FindElement<Button>(By.XPath(doneLocator))
                                   .WaitForDisplayed<Button>(timeout).Click();
            settings.Browser.Window.WaitForLoading(timeout);
            return new ComparationPage(timeout, loggers.loggers.ToArray(), make,model,year);
        }
        protected override string GetHeadder()
        {
            return settings.Browser.Window.FindElement<Text>(By.XPath(headdersLocator))
                                          .WaitForDisplayed<Text>(timeout).InnerHTML;
        }
    }
}