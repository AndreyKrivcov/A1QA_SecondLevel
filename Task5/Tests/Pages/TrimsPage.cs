using System;
using OpenQA.Selenium;
using SeleniumWrapper.Elements;
using SeleniumWrapper.Logging;
using Tests.Pages.Shared;

namespace Tests.Pages
{
    class TrimsPage : PageBase
    {
        public TrimsPage(int year, string make, string model,TimeSpan timeout, Logger[] loggers) : base(timeout, loggers)
        {
            Year = year;
            Make = make;
            Model = model;
        }

        private readonly string headderLocator = "//div[@class=\"trim-header__title\"]/h1";
        private readonly string engineLocator = "(//div[@class=\"trim-card\"])[2]/div[4]";
        private readonly string transmissionLocator = "(//div[@class=\"trim-card\"])[2]/div[5]";

        public int Year { get; }
        public string Make { get; }
        public string Model { get; }
        public string Engine
        {
            get
            {
                return settings.Browser.Window.FindElement<Contaner>(By.XPath(engineLocator))
                                              .WaitForDisplayed<Contaner>(timeout).InnerHTML;
            }
        }
        public string Transmission
        {
            get
            {
                return settings.Browser.Window.FindElement<Contaner>(By.XPath(transmissionLocator))
                                              .WaitForDisplayed<Contaner>(timeout).InnerHTML;
            }
        }
        protected override string GetHeadder()
        {
            return settings.Browser.Window.FindElement<Text>(By.XPath(headderLocator))
                                          .WaitForDisplayed<Text>(timeout).InnerHTML;
        }

        public static explicit operator ModelDetales(TrimsPage page)
        {
            return new ModelDetales
            {
                Engine = page.Engine,
                Transmission = page.Transmission,
                Make = page.Make,
                Model = page.Model,
                Year = page.Year
            };
        } 
    }
}