using System;
using OpenQA.Selenium;
using SeleniumWrapper.Elements;
using SeleniumWrapper.Logging;
using Tests.Pages.Shared;

namespace Tests.Pages
{
    class ResearchPage : PageBase
    {
        public ResearchPage(TimeSpan timeout, Logger[] loggers) : base(timeout,loggers)
        {
            CarsSelector = new CarsSelector(
                settings.Browser.Window.FindElement<Select>(By.XPath(makeLocator)).WaitForDisplayed<Select>(timeout),
                settings.Browser.Window.FindElement<Select>(By.XPath(modelLocator)).WaitForDisplayed<Select>(timeout),
                settings.Browser.Window.FindElement<Select>(By.XPath(yearLocator)).WaitForDisplayed<Select>(timeout),
                selectorsForbiddenValues
            );
        }

#region Locators
        private readonly string headerLocator = "//*[@id=\"root\"]//section//h1";
        private readonly string makeLocator = "//select[@name=\"makeId\"]";
        private readonly string modelLocator = "//select[@name=\"modelId\"]";
        private readonly string yearLocator = "//select[@name=\"year\"]";
        private readonly string searchBtn = "//select[@name=\"year\"]/following::div[1]/input";
        private readonly string sideBySideLocator = "//a[@data-linkname=\"compare-cars\"]";
#endregion

        private readonly ForbiddenValues selectorsForbiddenValues = new ForbiddenValues
        {
            Make = "All Makes",
            Model = "All Models",
            Year = "All Years"
        };

        public CarsSelector CarsSelector { get; }

        public CarPage Car()
        {            
            string make = CarsSelector.Make.Selected;
            string model = CarsSelector.Model.Selected;
            int year = Convert.ToInt32(CarsSelector.Year.Selected);
            settings.Browser.Window.FindElement<Button>(By.XPath(searchBtn)).WaitForDisplayed<Button>(timeout).Click();
            return new CarPage(year,make,model,timeout,loggers.loggers.ToArray());
        }
        public SideBySidePage SideBySide
        {
            get
            {
                settings.Browser.Window.FindElement<Button>(By.XPath(sideBySideLocator)).WaitForDisplayed<Button>(timeout).Click();
                return new SideBySidePage(timeout,loggers.loggers.ToArray());
            }
        }

        protected override string GetHeadder()
        {
            string header = settings.Browser.Window.FindElement<GenericElement>(By.XPath(headerLocator))
                                                    .WaitForDisplayed<GenericElement>(timeout).InnerHTML;
            return header.Split("<span>")[0];
        }
    }
}