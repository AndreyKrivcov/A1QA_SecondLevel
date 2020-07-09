using System;

using OpenQA.Selenium;
using SeleniumWrapper.Elements;
using SeleniumWrapper.Logging;
using Tests.Pages.Shared;

namespace Tests.Pages
{
    class CarPage : PageBase
    {
        public CarPage(int year, string make, string model, TimeSpan timeout, Logger[] loggers) : base(timeout,loggers)
        {
            Year = year;
            Make = make;
            Model = model;
        }

        private readonly string trimsLocator = "//a[@data-linkname=\"trim-compare\"]";
        private readonly string headderLocator = "//h1[@class=\"cui-page-section__title\"]";

        public int Year { get; }
        public string Make { get; }
        public string Model { get; }

        private Link Trims => settings.Browser.Window.FindElement<Link>(By.XPath(trimsLocator));

        public bool IsTrimAvailible 
        {
            get
            {
                try
                {
                    return Trims.IsExists;
                }
                catch
                {
                    return false;
                }
            }
        }
        public TrimsPage Compare2Trim 
        {
            get
            {
                Trims.Click();
                return new TrimsPage(Year,Make,Model,timeout,loggers.loggers.ToArray());
            }
        }
        protected override string GetHeadder()
        {
            return settings.Browser.Window.FindElement<Text>(By.XPath(headderLocator))
                                          .WaitForDisplayed<Text>(timeout).InnerHTML;
        }
    }
}